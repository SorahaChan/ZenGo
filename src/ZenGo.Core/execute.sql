DROP DATABASE IF EXISTS `zen_sql_ap1`;
CREATE DATABASE IF NOT EXISTS `zen_sql_ap1`;
USE `zen_sql_ap1`;
CREATE TABLE `players` (
                           `user_id` BIGINT unsigned NOT NULL,
                           `exp` BIGINT NOT NULL,
                           `set_weapon` INT,
                           `set_armor_head` INT,
                           `set_armor_chest` INT,
                           `set_armor_boots` INT,
                           PRIMARY KEY (`user_id`)
);
CREATE TABLE `items` (
    
                         `user_id` BIGINT unsigned NOT NULL,
                         `item_id` INT NOT NULL,
                         `quantity` INT NOT NULL,
                         PRIMARY KEY (`user_id`),
                         KEY `user_index` (`user_id`) USING BTREE
);
CREATE TABLE `weapons` (
                           `index` INT NOT NULL AUTO_INCREMENT,
                           `user_id` BIGINT unsigned NOT NULL,
                           `weapon_id` INT NOT NULL,
                           `exp` INT NOT NULL,
                           `enchant_1` INT,
                           `enchant_2` INT,
                           `enchant_3` INT,
                           PRIMARY KEY (`index`)
);
CREATE TABLE `armors` (
                          `index` INT NOT NULL AUTO_INCREMENT,
                          `user_id` BIGINT unsigned NOT NULL,
                          `armor_id` INT NOT NULL,
                          `armor_type` INT NOT NULL,
                          `exp` INT NOT NULL,
                          `enchant` INT,
                          PRIMARY KEY (`index`)
);
CREATE TABLE `battle_data` (
                               `user_id` BIGINT unsigned NOT NULL,
                               `channel_id` BIGINT unsigned NOT NULL,
                               `user_hp` BIGINT NOT NULL,
                               PRIMARY KEY (`user_id`)
);
CREATE TABLE `channel_data` (
                                `channel_id` BIGINT unsigned NOT NULL,
                                `guild_id` BIGINT unsigned NOT NULL,
                                `monster_id` INT NOT NULL,
                                `monster_level` BIGINT NOT NULL,
                                `monster_hp` BIGINT NOT NULL,
                                PRIMARY KEY (`channel_id`)
);
