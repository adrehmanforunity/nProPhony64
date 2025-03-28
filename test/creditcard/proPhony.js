	var wsImpl = window.WebSocket || window.MozWebSocket;
	let sampleAgentsTable ='{"agents":[{"RowID":"13","LoginName":"OVS","FirstName":"OVS","LastName":"System","QueueID":"1","Ext":"1040","ExtSecret":"ab0000","Password":"hello","FirstTime":"0","CreatedOn_DateTime":"1/12/2021 6:20:37 PM","PasswordOn_DateTime":"1/12/2021 6:20:37 PM","CreatedBy_User":"0","Supervisor":"1","Status":"1"},{"RowID":"19","LoginName":"CSO.01","FirstName":"CSO","LastName":"1","QueueID":"1","Ext":"1071","ExtSecret":"ab0000","Password":"hello","FirstTime":"0","CreatedOn_DateTime":"1/12/2021 6:20:37 PM","PasswordOn_DateTime":"3/1/2021 10:28:40 AM","CreatedBy_User":"0","Supervisor":"0","Status":"1"},{"RowID":"20","LoginName":"CSO.02","FirstName":"CSO","LastName":"2","QueueID":"1","Ext":"1074","ExtSecret":"ab0000","Password":"hello","FirstTime":"0","CreatedOn_DateTime":"1/12/2021 6:20:37 PM","PasswordOn_DateTime":"3/10/2021 3:55:58 PM","CreatedBy_User":"1","Supervisor":"0","Status":"1"}]}';
	let agTable = JSON.parse(sampleAgentsTable);
	
	//let text = '{ "employees" : [' +
	//'{ "firstName":"John" , "lastName":"Doe" },' +
	//'{ "firstName":"Anna" , "lastName":"Smith" },' +
	//'{ "firstName":"Peter" , "lastName":"Jones" } ]}';
	//const obj = JSON.parse(text);
	function updateConnectionStatus(statusText)
	{
		var inc = document.getElementById('connectionStatus');
		inc.innerHTML ='';
		inc.innerHTML = statusText;
	}
	function appendToScreen(msg)
	{
		try {
			var inc = document.getElementById('incomming');
			var msgReceved=msg;
			var existingDataOnPage=inc.innerHTML;
			inc.innerHTML ='';
			inc.innerHTML = msgReceved + '<br/>' + existingDataOnPage;
		}catch(err) {
			throw 'appendToScreen (' + "Error=" + err.message + ")";
			//return false;
		}
	}

	function echotestProPhony(msg)
	{
    	sendCommand(msg);
	}


	function connectProPhony()
	{
		try {

			appendToScreen('Connecting...');
			window.ws = new wsImpl('ws://localhost:8181/');
				// when data is comming from the server, this metod is called
				ws.onmessage = function (evt) {
					try {
						console.log(evt.data)
						let dataRececeid = JSON.parse(evt.data);
					} catch (error) {
						console.log(error);
					}
					appendToScreen(evt.data);
				};



				// when the connection is established, this method is called
				ws.onopen = function () {
					appendToScreen('Connected');
					updateConnectionStatus('Connected');
					document.getElementById('connectionStatus').style="background-color: green"
					document.getElementById("btnDisconnect").disabled = false;
					document.getElementById("btnConnect").disabled = true;
				};
				// when the connection is closed, this method is called
				ws.onclose = function () {
					appendToScreen('Disconnected');
					updateConnectionStatus('Disconnected');
					document.getElementById('connectionStatus').style="background-color: red"
					document.getElementById("btnDisconnect").disabled = true;
					document.getElementById("btnConnect").disabled = false;
				};

            var form = document.getElementById('proPhonyControls');
			form.addEventListener('submit', function(e){e.preventDefault();});
			return true;
		}catch(err) {
			throw 'connectProPhony failed(' + "Error=" + err.message + ")";
			//return false;
		}
	}

	function disconnectProPhony()
	{
		try {

			if (ws.readyState==0 || ws.readyState==1)
			{
				appendToScreen('Disconnecting..');

			} else 
			{
				throw 'disconnectProPhony(' + "Error=" + 'not connected' + ")";
			}
    		ws.close();
    		return true;
    	}catch(err) {
			throw 'disconnectProPhony(' + "Error=" + err.message + ")";
			//return false;
		}
	}

	function sendCommand(cmd)
	{
		try {
			var ts = new Date().getTime();
    		ws.send('ts{' + ts + '} ' + 'cmd{' + cmd + '} ');
    	}catch(err) {
			throw 'sendCommand(' + "Error=" + err.message + ")";
		}
	};
	