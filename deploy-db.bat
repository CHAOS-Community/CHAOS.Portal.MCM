REM tools\chaos.deployment\Chaos.Deployment.UI.Console.exe -cs "user id=chaos;password=CHAOSpbvu7000;server=larm.cpwvkgghf9fg.eu-west-1.rds.amazonaws.com;persist security info=True;database=MCM;Allow User Variables=True" -a deploy-tables           -p sql\1.baseline\tables
tools\chaos.deployment\Chaos.Deployment.UI.Console.exe -cs "user id=chaos;password=CHAOSpbvu7000;server=larm.cpwvkgghf9fg.eu-west-1.rds.amazonaws.com;persist security info=True;database=MCM;Allow User Variables=True" -a deploy-storedprocedures -p sql\3.storedprocedures
tools\chaos.deployment\Chaos.Deployment.UI.Console.exe -cs "user id=chaos;password=CHAOSpbvu7000;server=larm.cpwvkgghf9fg.eu-west-1.rds.amazonaws.com;persist security info=True;database=MCM;Allow User Variables=True" -a deploy-views            -p sql\4.views
REM tools\chaos.deployment\Chaos.Deployment.UI.Console.exe -cs "user id=glomex;password=ChetR3ph;server=stagemysql00.cpwvkgghf9fg.eu-west-1.rds.amazonaws.com;persist security info=True;database=glomexmcm;Allow User Variables=True" -a import-scripts          -p sql\6.data\initial.sql