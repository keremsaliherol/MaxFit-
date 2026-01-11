@echo off
echo Running database updates...
echo.

sqlcmd -S KEREM\MSSQLSERVER2017 -d MaxFit -E -i "AddPaymentAndProfilePhoto.sql"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Database updated successfully!
    echo You can now run: dotnet run
) else (
    echo.
    echo Error: Failed to update database
    echo Please run the SQL script manually in SQL Server Management Studio
)

pause
