using Discord;
using Newtonsoft.Json;
using ZenGo.Core.Entities;
using ZenGo.Core.Entities.Items;
using ZenGo.Core.Entities.Models;
using ZenGo.Core.Entities.Results;
using ZenGo.Core.Extensions;

namespace ZenGo.Core;

public sealed class ZenGoService
{
    private readonly DatabaseService _database;

    private static IReadOnlyList<Monster> _monsters;

    private const double RareRate = 0.05;

    private const double UnknownRate = 0.001;

    public ZenGoService(ZenGoServiceConfig config)
    {
        _database = new DatabaseService(config.DatabaseConnection ?? throw new FormatException("Invalid Database Connection"));

        var text = File.ReadAllText(config.MonsterJsonPath ?? throw new FormatException("Invalid Json"));
        
        _monsters = JsonConvert.DeserializeObject<IReadOnlyList<Monster>>(text);
    }

    public static IReadOnlyCollection<IItem> Items = new List<IItem> { new Elixir(), new FireBook() };

    public async Task<IProcessResult> UseAttackAsync(IUser user, ITextChannel channel)
    {
        // 攻撃こまんど
        return await AttackAsync(user, channel).ConfigureAwait(false);
    }

    public async Task<IProcessResult> UseItemAsync(IUser user, ITextChannel channel, IItem item)
    {
        // あいてむこまんど
        if (item is null) return new ErrorResult("Not Found Item");

        return await ItemAsync(user, channel, item).ConfigureAwait(false);
    }
    
    public async Task<IProcessResult> UseItemAsync(IUser user, ITextChannel channel, string itemName)
    {
        // あいてむこまんど
        var item = Items.FirstOrDefault(x => x.Prefix.Contains(itemName));
        
        if (item is null) return new ErrorResult("Not Found Item");

        return await ItemAsync(user, channel, item).ConfigureAwait(false);
    }
    
    public async Task<IProcessResult> UseResetAsync(IUser user, ITextChannel channel)
    {
        // りせっとこまんど
        return await ResetAsync(user, channel).ConfigureAwait(false);
    }

    public async Task<IProcessResult> UseInquiryAsync(ITextChannel channel)
    {
        return await InquiryAsync(channel).ConfigureAwait(false);
    }

    public async Task<IProcessResult> UseProfileAsync(IUser user)
    {
        return await ProfileAsync(user).ConfigureAwait(false);
    }

    private async Task<(bool, BattleData)> IsValidBattleAsync(ulong userId, ulong channelId)
    {
        // 指定したチャンネルで戦闘中または非戦闘かどうか, かつ戦闘中のとき生きているか
        var battle = await _database.FetchBattleDataAsync(userId);

        return (battle is null || battle.ChannelId == channelId && IsAlive(battle), battle);
    }

    private static bool IsAlive(BattleData data)
    {
        // 戦闘中のとき生きているか
        return data.UserHp > 0;
    }

    private static bool IsAlive(ChannelData data)
    {
        // 敵が生きているか
        return data.MonsterHp > 0;
    }
    
    private async Task<IProcessResult> AttackAsync(IUser user, ITextChannel channel)
    {
        // 内部攻撃処理
        var (isValid, battleData) = await IsValidBattleAsync(user.Id, channel.Id);
        
        if (!isValid) return new ErrorResult("Invalid Channel or Player's hp less than 0.");
        
        Player player = await _database.FetchPlayerAsync(user.Id) ?? new Player(user.Id);

        var level = player.GetLevel();
        
        battleData ??= new BattleData(user.Id, channel.Id, player.GetDefaultHp());

        ChannelData channelData = await _database.FetchChannelDataAsync(channel.Id)
                                  ?? new ChannelData(channel.Id, channel.GuildId, 1, 1, 55);

        Monster monster = _monsters.Single(x => x.Id == channelData.MonsterId);
        
        var damage = GiveDamage(level, channelData, monster);

        if (IsAlive(channelData))
        {
            // 倒せなかったとき
            var receive = ReceiveDamage(battleData, channelData, monster);

            await SaveBattleAsync(player, battleData, channelData);

            var damageLog =
                $"{user.Mention} attacked to `{monster.Name}`!\n`{monster.Name}` health: {channelData.MonsterHp} ({damage} damage)\n\n" +
                $"`{monster.Name}` attacked to {user.Mention}!\n{user.Mention} health: {battleData.UserHp} / {player.GetDefaultHp()} ({receive} damage)\n\n";

            return new AttackResult(damageLog);
        }
        else
        {
            // 倒せたとき
            var damageLog = $"{user.Mention} attacked to `{monster.Name}`! `{monster.Name}` health: 0 ({damage} damage)";
            
            return await SetWinAsync(damageLog, player, channelData, monster, channel);
        }
    }
    
    private async Task<IProcessResult> ItemAsync(IUser user, ITextChannel channel, IItem item, int step = 1)
    {
        // 内部アイテム処理
        var itemDb = await _database.FetchItemAsync(user.Id, item.Id);

        if (itemDb is null || itemDb.Quantity == 0) return new ErrorResult($"You don't have {item.DisplayName}.");

        var result = item switch
        {
            IMagic magic => await MagicAsync(user, channel, magic),
            
            Elixir => await ElixirAsync(channel),
            
            _ => new ErrorResult("Unknown Item")
        };

        if (result is not ErrorResult && item.IsDecrease)
        {
            itemDb.Quantity -= step;

            await _database.UpdateItemAsync(itemDb);
        }

        return result;
    }
    
    private async Task<IProcessResult> AttackWithItemAsync(IUser user, ITextChannel channel, IItem item)
    {
        // 内部攻撃アイテム処理
        switch (item)
        {
            default: return new ErrorResult("Unknown Item");
        }
    }

    private async Task<IProcessResult> MagicAsync(IUser user, ITextChannel channel, IMagic magic)
    {
        // 内部FB処理
        var (isValid, battleData) = await IsValidBattleAsync(user.Id, channel.Id);
        
        if (!isValid) return new ErrorResult("Invalid Channel or Player's hp less than 0.");
        
        Player player = await _database.FetchPlayerAsync(user.Id) ?? new Player(user.Id);

        var level = player.GetLevel();
        
        battleData ??= new BattleData(user.Id, channel.Id, player.GetDefaultHp());

        ChannelData channelData = await _database.FetchChannelDataAsync(channel.Id)
                                  ?? new ChannelData(channel.Id, channel.GuildId, 1, 1, 55);

        Monster monster = _monsters.Single(x => x.Id == channelData.MonsterId);
        
        var damage = GiveMagicDamage(level, channelData, monster, magic.MagicEffect);

        if (IsAlive(channelData))
        {
            // 倒せなかったとき
            await SaveBattleAsync(player, battleData, channelData);

            var damageLog = $"{user.Mention} use {magic.DisplayName} to `{monster.Name}`!\n" +
                $"`{monster.Name}` health: {channelData.MonsterHp:#,0} ({damage:#,0} damage)\n\n";

            return new AttackResult(damageLog);
        }
        else
        {
            // 倒せたとき
            var damageLog = $"{user.Mention} use {magic.DisplayName} to `{monster.Name}`!\n" +
                $"`{monster.Name}` health: 0 ({damage:#,0} damage)";
            
            return await SetWinAsync(damageLog, player, channelData, monster, channel);
        }
    }
    
    private async Task<IProcessResult> ElixirAsync(ITextChannel channel)
    {
        // 内部エリクサー処理
        List<BattleData> list = await _database.FetchBattleDataListAsync(channel.Id);
        
        if (list is null || list.Count == 0) return new ErrorResult("Channel Battle is not active.");

        List<Player> players = await _database.FetchPlayersAsync(list);

        foreach (var battleData in list)
        {
            battleData.UserHp = players.Single(x => x.UserId == battleData.UserId).GetDefaultHp();
        }

        await _database.UpdateBattleDataListAsync(list);

        return new ItemResult("All players health have been recovered!");
    }
    
    private async Task<IProcessResult> ResetAsync(IUser user, ITextChannel channel)
    {
        // 内部りせっと処理
        List<BattleData> list = await _database.FetchBattleDataListAsync(channel.Id);
        if (list.Count == 0)
        {
            return new ErrorResult("Channel Battle is not active.");
        }
        else
        {
            if (list.All(x => x.UserId != user.Id))
            {
                return new ErrorResult("Reset Users must be in channel battle.");
            }
            else
            {
                ChannelData channelData = await _database.FetchChannelDataAsync(channel.Id);
                Monster monster = await ResetMonsterAsync(channelData);

                var r = $"{user.Mention} is time traveler...!";

                return new ResetResult(r, GetAppearMessage(monster, channelData), monster.ImageUrl);
            }
        }
    }
    
    private async Task<IProcessResult> InquiryAsync(ITextChannel channel)
    {
        // 内部いんく処理
        ChannelData channelData = await _database.FetchChannelDataAsync(channel.Id);

        if (channelData is null) return new ErrorResult("Database Not Found");

        Monster monster = _monsters.Single(x => x.Id == channelData.MonsterId);

        var message = $"Inquiry of {channel.Mention}\n\n`{monster.Name}`\n\n" +
                      $"level `{channelData.MonsterLevel}` health `{channelData.MonsterHp}`\n\n";

        List<BattleData> list = await _database.FetchBattleDataListAsync(channel.Id);
        if (list is null || list.Count == 0)
        {
            message += "No players in battle";
            return new InquiryResult(message, monster.ImageUrl);
        }
        else
        {
            message = list.Aggregate(message, (x, battleData) => x + $"<@{battleData.UserId}> hp `{battleData.UserHp}`\n");

            return new InquiryResult(message, monster.ImageUrl);
        }
    }

    private async Task<IProcessResult> ProfileAsync(IUser user)
    {
        // 内部ステータス処理
        Player player = await _database.FetchPlayerAsync(user.Id);
        
        if (player is null) return new ErrorResult("Not Found User Profile");

        var message = $"`{user}`'s Profile\n\nlevel `{player.GetLevel():#,0}` exp `{player.Exp:#,0}`\n\n";

        List<Item> items = await _database.FetchItemsAsync(user.Id);

        if (items is null) message += "No Items";
        else
        {
            var itemMessage = String.Empty;
            
            foreach (var itemDb in items)
            {
                var item = Items.FirstOrDefault(x => x.Id == itemDb.ItemId);
                if (item is not null && itemDb.Quantity != 0)  itemMessage += $"+ {item.DisplayName} {itemDb.Quantity}\n";
            }

            if (String.IsNullOrEmpty(itemMessage))
            {
                message += "No Items";
            }
            else
            {
                message += $"```diff\n{itemMessage}```";
            }
        }

        return new ProfileResult(message);
    }
    
    private int GiveDamage(int level, ChannelData channelData, Monster monster)
    {
        // ダメージを与える
        var random = new Random();
        var damage = monster.Rare switch
        {
            MonsterRare.Strong => (int) (level * (random.NextSingle() / 2) + 10 + level),
            MonsterRare.Unknown => (int) (level * (random.NextSingle() / 10) + 10 + level),
            _ => (int) (level * (random.NextSingle() / 1.5) + 10 + level)
        };

        channelData.MonsterHp -= damage;

        return damage;
    }
    
    private int GiveMagicDamage(int level, ChannelData channelData, Monster monster, double magicEffect = 1.0)
    {
        // ダメージを与える
        var random = new Random();
        var damage = (int) ((random.NextDouble() / 2.5 + 0.6) * level * magicEffect);

        channelData.MonsterHp -= damage;

        return damage;
    }
    
    private int ReceiveDamage(BattleData battleData, ChannelData channelData, Monster monster)
    {
        // ダメージを受け取る
        var random = new Random();
        var damage = monster.Rare switch
        {
            MonsterRare.Strong => (int) (channelData.MonsterLevel * (1 + random.NextSingle()) * 2),
            MonsterRare.Unknown => (int) (channelData.MonsterLevel * (1 + random.NextSingle()) * 10),
            _ => (int) (channelData.MonsterLevel * (2 + random.NextSingle()) + 5)
        };

        battleData.UserHp -= battleData.UserHp < damage ? battleData.UserHp : damage;

        return damage;
    }
    
    private async Task<DefeatResult> SetWinAsync(string damageLog, Player player, ChannelData channelData, Monster monster, ITextChannel channel)
    {
        // 勝利処理
        var expMessage = await SetExpAndClearBattleAsync(player, channelData, monster, channel);
        
        Monster nextMonster = await NextMonsterAsync(channelData);

        return new DefeatResult(damageLog, expMessage, GetAppearMessage(nextMonster, channelData), nextMonster.ImageUrl);
    }
    
    private async Task<string> SetExpAndClearBattleAsync(Player lastAttacker, ChannelData channelData, Monster monster, ITextChannel channel)
    {
        // 経験値付与処理
        string expMessage = $"`{monster.Name}` has been defeated!\n\n";
        
        List<BattleData> battles = await _database.FetchBattleDataListAsync(channel.Id);

        List<Player> players = await _database.FetchPlayersAsync(battles);
        
        if (players.All(x => x.UserId != lastAttacker.UserId)) players.Add(lastAttacker);

        var exp = monster.Rare switch
        {
            MonsterRare.Strong => channelData.MonsterLevel * 5,
            MonsterRare.Rare => channelData.MonsterLevel * 10,
            MonsterRare.Unknown => channelData.MonsterLevel * 100,
            _ => channelData.MonsterLevel
        };

        foreach (var player in players)
        {
            expMessage += $"<@{player.UserId}> got {exp:#,0} Exp.\n";
            
            var oldLevel = player.GetLevel();

            player.Exp += exp;
            
            var newLevel = player.GetLevel();

            if (newLevel > oldLevel) expMessage += $"<@{player.UserId}> leveled up from {oldLevel:#,0} to {newLevel:#,0}!\n";
        }

        await _database.UpdatePlayersAsync(players);
        
        await _database.ClearBattleDataListAsync(battles);

        return expMessage;
    }
    
    private async Task<Monster> NextMonsterAsync(ChannelData channelData, int step = 1)
    {
        // 次の敵を出す処理
        channelData.MonsterLevel += step;

        var monsters = _monsters.OrderBy(_ => Guid.NewGuid());

        var monsterRand = new Random().NextDouble();

        Monster monster = channelData.MonsterLevel % 5 == 0
            ? monsters.Single(x => x.Rare == MonsterRare.Strong)
            : monsterRand switch
            {
                <= UnknownRate => monsters.Single(x => x.Rare == MonsterRare.Unknown),
                <= RareRate => monsters.Single(x => x.Rare == MonsterRare.Rare),
                _ => monsters.Single(x => x.Rare == MonsterRare.Normal)
            };

        channelData.MonsterId = monster.Id;
        
        channelData.MonsterHp = monster.Rare switch  
            {
                MonsterRare.Rare => channelData.MonsterLevel * 10 + 50,
                MonsterRare.Unknown => channelData.MonsterLevel * 100 + 50,
                _ => channelData.MonsterLevel * 5 + 50
            };

        await _database.UpdateChannelDataAsync(channelData);

        return monster;
    }

    private async Task<Monster> ResetMonsterAsync(ChannelData channelData)
    {
        Monster monster = _monsters.Single(x => x.Id == channelData.MonsterId);

        if (monster.Rare is MonsterRare.Rare or MonsterRare.Unknown)
        {
            monster = _monsters.OrderBy(_ => Guid.NewGuid()).Single(x => x.Rare == MonsterRare.Normal);

            channelData.MonsterId = monster.Id;
        }
        
        channelData.MonsterHp = monster.Rare switch
        {
            MonsterRare.Rare => channelData.MonsterLevel * 10 + 50,
            MonsterRare.Unknown => channelData.MonsterLevel * 100 + 50,
            _ => channelData.MonsterLevel * 5 + 50
        };

        await _database.ClearBattleDataListAsync(channelData.ChannelId);
        
        await _database.UpdateChannelDataAsync(channelData);

        return monster;
    }

    private string GetAppearMessage(Monster monster, ChannelData channelData)
    {
        return $"`{monster.Name}` is waiting...!\n\nrare: `{monster.Rare}`\n\n" +
               $"level `{channelData.MonsterLevel:#,0}` health `{channelData.MonsterHp:#,0}`";
    }

    private async Task SaveBattleAsync(Player player, BattleData battleData, ChannelData channelData)
    {
        await _database.UpdatePlayerAsync(player);

        await _database.UpdateBattleDataAsync(battleData);

        await _database.UpdateChannelDataAsync(channelData);
    }
}