DROP DATABASE IF EXISTS `sugarsql`;
CREATE DATABASE IF NOT EXISTS `sugarsql`;

USE `sugarsql`;

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
	`Id` CHAR(36) NOT NULL COMMENT 'Id' COLLATE 'utf8mb4_general_ci',
	`Name` VARCHAR(64) NOT NULL COMMENT '姓名' COLLATE 'utf8mb4_general_ci',
	`Age` INT(3) NOT NULL COMMENT '年龄',
	`Email` VARCHAR(64) NULL DEFAULT NULL COMMENT '邮箱' COLLATE 'utf8mb4_general_ci',
	`Phone` VARCHAR(64) NULL DEFAULT NULL COMMENT '电话' COLLATE 'utf8mb4_general_ci',
	`CreationTime` DATETIME NOT NULL COMMENT '创建时间',
	`CreatorId` CHAR(36) NULL DEFAULT NULL COMMENT '创建人' COLLATE 'utf8mb4_general_ci',
	`LastModificationTime` DATETIME NULL DEFAULT NULL COMMENT '更新时间',
	`LastModifierId` CHAR(36) NULL DEFAULT NULL COMMENT '更新人' COLLATE 'utf8mb4_general_ci',
	`IsDeleted` TINYINT(2) NOT NULL DEFAULT '0' COMMENT '是否删除，0：未删除，1：已删除',
	`DeleterId` CHAR(36) NULL DEFAULT NULL COMMENT '删除人' COLLATE 'utf8mb4_general_ci',
	`DeletionTime` DATETIME NULL DEFAULT NULL COMMENT '删除时间',
	PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

