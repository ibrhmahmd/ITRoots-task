IF NOT EXISTS (
    SELECT name
    FROM sys.databases
    WHERE name = N'IT_Roots_Task_Db'
) BEGIN CREATE DATABASE IT_Roots_Task_Db;
END
GO