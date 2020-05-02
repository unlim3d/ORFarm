 
:st
set process1="deadlinewebservice.exe"
set process2="Node.exe"
set process3="NoDeadLine.exe"
 
cls
 

tasklist | find /I %process1%
set res1=%errorlevel%

tasklist | find /I %process2%
set res2=%errorlevel%

tasklist | find /I %process3%
set res3=%errorlevel%   
 

if %res1% EQU 1 ( 
 
 start "DeadLineWebService" "c:\Program Files\Thinkbox\Deadline10\bin\deadlinewebservice.exe"
)

 
  
if %res2% EQU 1 ( 
 start "Node" "node start"
 start "NodeRenderFarmService" "node C:\Megarender-full\server.js"
 start "YGBOT" "node c:\Bot\query\queryChecker-master\queryChecker-master\index.js"
)

if %res3% EQU 1 ( 
 
 start "FarmManager" "c:\Megarender-full\NoDeadLine\bin\Release\netcoreapp3.1\publish\NoDeadLine.exe"
)
 
 
 
 timeout /t 60

goto :st
exit