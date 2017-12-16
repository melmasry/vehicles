del Output.txt
for %%G in (*.sql) do sqlcmd -S "(local)" -i "%%G" >> Output.txt
ECHO Database created successfully... >> Output.txt
pause