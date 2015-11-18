-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Oct 04, 2015 at 05:32 AM
-- Server version: 5.6.17
-- PHP Version: 5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `dotabound`
--

-- --------------------------------------------------------

--
-- Table structure for table `playerinfo`
--

CREATE TABLE IF NOT EXISTS `playerinfo` (
  `_id` int(11) NOT NULL AUTO_INCREMENT,
  `_username` varchar(50) DEFAULT NULL,
  `_password` varchar(50) DEFAULT NULL,
  `_win` int(12) DEFAULT '0',
  `_lose` int(12) DEFAULT '0',
  PRIMARY KEY (`_id`),
  UNIQUE KEY `_id` (`_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=8 ;

--
-- Dumping data for table `playerinfo`
--

INSERT INTO `playerinfo` (`_id`, `_username`, `_password`, `_win`, `_lose`) VALUES
(1, 'tri', 'tri', 0, 0),
(5, 'garret', '123456', 0, 0),
(6, 'trung', '123456', 0, 0),
(7, 'truc', '123456', 0, 0);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
