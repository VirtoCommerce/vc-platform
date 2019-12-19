# Launch and send to background
/opt/mssql/bin/sqlservr &
pid=$!

# wait for all databases to be online
export STATUS=1
i=0

while [[ $STATUS -ne 0 ]] && [[ $i -lt 30 ]]; do
	i=$i+1
	/opt/mssql-tools/bin/sqlcmd -t 1 -U sa -P $SA_PASSWORD -Q "select case when exists(select * from sys.databases where state <> 0) then 0 else 1 end;" >> /dev/null
	STATUS=$?
done

if [ $STATUS -ne 0 ]; then
	echo "Error: mssql took too long to start up, exiting..."
	exit 1
fi

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
/opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -l 30 -e -i $DIR/mssql-init.sql

# Wait indefinitely on the sqlserver process
wait $pid
