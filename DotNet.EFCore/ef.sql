DROP DATABASE IF EXISTS `efsql`;
CREATE DATABASE IF NOT EXISTS `efsql`;

USE `efsql`;

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `Id` char(36) NOT NULL COMMENT 'Id',
  `Name` varchar(64) NOT NULL COMMENT '姓名',
  `Age` int(3) NOT NULL COMMENT '年龄',
  `Email` varchar(64) DEFAULT NULL COMMENT '邮箱',
  `Phone` varchar(64) DEFAULT NULL COMMENT '电话',
  `CreationTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` char(36) DEFAULT NULL COMMENT '创建人',
  `LastModificationTime` datetime DEFAULT NULL COMMENT '更新时间',
  `LastModifierId` char(36) DEFAULT NULL COMMENT '更新人',
  `IsDeleted` tinyint(2) NOT NULL DEFAULT '0' COMMENT '是否删除，0：未删除，1：已删除',
  `DeleterId` char(36) DEFAULT NULL COMMENT '删除人',
  `DeletionTime` datetime DEFAULT NULL COMMENT '删除时间',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;