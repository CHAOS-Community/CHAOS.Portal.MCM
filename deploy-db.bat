tools\chaos.deployment\Chaos.Deployment.UI.Console.exe -cs "user id=dev_chaos;password=pbvu7000;server=172.18.3.1;persist security info=True;database=devmcm;Allow User Variables=True" -a deploy-tables           -p sql\1.baseline\tables
tools\chaos.deployment\Chaos.Deployment.UI.Console.exe -cs "user id=dev_chaos;password=pbvu7000;server=172.18.3.1;persist security info=True;database=devmcm;Allow User Variables=True" -a deploy-storedprocedures -p sql\3.storedprocedures
tools\chaos.deployment\Chaos.Deployment.UI.Console.exe -cs "user id=dev_chaos;password=pbvu7000;server=172.18.3.1;persist security info=True;database=devmcm;Allow User Variables=True" -a deploy-views            -p sql\4.views
tools\chaos.deployment\Chaos.Deployment.UI.Console.exe -cs "user id=dev_chaos;password=pbvu7000;server=172.18.3.1;persist security info=True;database=devmcm;Allow User Variables=True" -a import-scripts          -p sql\6.data\initial.sql