// Setup basic express server
var express = require('express');
var app = express();
var path = require('path');
var server = app.listen(8080);
var io = require('socket.io').listen(server);
var port = process.env.PORT || 8080;
var mysql = require('mysql'); 
var con = mysql.createConnection({host: "localhost", user: "root", password: "", database:"xamarinusers"});
var table = "user";

var disconnected = [];
var Players = [];//player_id, socket_id, pseudo, room_id
var Rooms = [];  //id, name, password, players


server.listen(port, () => {
	console.log('Server listening at port %d', port);
});
io.on('connection', (socket) => {
	
	app.use(express.static(path.join(__dirname, 'public')));

	WriteConsole("connection +");
	socket.on('playerConnection', player =>{
		socket.username = player.Pseudo;      
		if(disconnected.includes(player)){
			console.log("aksjdaljsdhahdlkjfghkadshfglksdhfjgslkhdfgjlshkjfdgjslkhdfljkghsdfjgs");
		}
		Players.push({player_id:player.Id, socket_id:socket.id, pseudo:player.Pseudo, room_id:null});
		WriteConsole("connection : "+player.Pseudo);
		socket.emit('playerConnection', true);
	});
	socket.on('disconnect', () => {
		for (let i = Players.length-1; i >= 0 ; i--) 
			if(Players[i].socket_id == socket.id) {
				if (Players[i].room != null)
					socket.broadcast.to(Players[i].room_id).emit('playerLeaveRoom', Players[i].player_id);	
				disconnected.push(Players[i]);
				Players.splice(i, 1);
			}  
		console.log("--> Disconnection, "+ViewPlayers(Players)+" -> "+socket.username);
	});

	socket.on('loginPlayer', (player) => {
		var player = JSON.parse(player);
		console.log(player["Pseudo"]);
		con.connect(function(err) {	
			con.query("SELECT * FROM `"+table+"` WHERE pseudo = \""+player.Pseudo+"\" LIMIT 1", function (err, result, fields) {
				var return_value = "Error: error.";
				if(result == undefined){
					console.log("bdd inacessible");
					return_value = "Error: La base de donnée est innacessible."
				}
				else if (result[0]!=undefined)
					if (result[0].Password == player.Password)
					{
						return_value = result[0];
						WriteConsole("connection db : "+result[0].Pseudo);
						for (let i = Players.length-1; i >= 0; i--) 
							if(socket.id == Players[i].socket_id)
							{
								console.log(ViewPlayers(Players) +" : "+ socket.username+" = "+result[0].Pseudo+";");
								socket.username = result[0].Pseudo;
								for (let i = Players.length-1; i >= 0 ; i--) 
									if(Players[i].socket_id == socket.id) 
										Players.splice(i, 1);
							}
					
						Players.push({player_id:result[0].Id, socket_id:socket.id, pseudo:result[0].Pseudo, room_id:null});
						//console.log(ViewPlayers(Players));
					} 
					else
						return_value = "Error: Le mot de passe est incorrecte";
				else
					return_value = "Error: Ce pseudo n'existe pas";
				socket.emit("loginPlayerEcho", return_value);
			}); 
		});
	});
			

	socket.on("updateRoom", (room) => {//the host update the room
		io.sockets.adapter.rooms[room.Name].Players = room.Players;
		socket.broadcast.to(room.Name).emit('updateRoom', room);	
	});

	socket.on('createRoom', room =>{
		WriteConsole("\n->> createRoom:"+room.Name+", "+room.Players.length+", "+room.Password);
		//console.log(room);//console.log(io.sockets.adapter.rooms);
		if(!Rooms.includes(room)){
			socket.join(room.Name);//first join
			Rooms.push(room);
			io.sockets.adapter.rooms[room.Name].Name = room.Name;
			io.sockets.adapter.rooms[room.Name].Password = room.Password;
			io.sockets.adapter.rooms[room.Name].Players = room.Players;
			socket.emit("createRoomEcho", true);
			socket.broadcast.to(room.Name).emit("newRoomEcho", room);//todo
		}
		else
			socket.emit("createRoomEcho", "Error: Une room porte déjà ce nom.");
	});

	socket.on('getAllRoom', function(){
		WriteConsole("\n->> getAllRoom:\n"+"Rooms[].name");//dsfaésdfizgalduigfliae
		var result = [];
		Rooms.forEach(_Room => {
			var room = [];
			room = io.sockets.adapter.rooms[_Room.Name];
			if(room!=null) 
				result.push(room); 
		});
		console.log(ViewRooms(Rooms));
		socket.emit("getAllRoomEcho", result);
	});

	socket.on('joinRoom', data =>{
		WriteConsole("\n->> joinRoom");
		//if (data.Room != Rooms[i])
		ThisRoom = false;
		for (i = 0;i<Rooms.length;i++){
			if(Rooms[i].Name == data.Room.Name){
				ThisRoom = Rooms[i];
			} 
		}
		  
		console.log(ThisRoom);
		WriteConsole(ThisRoom.Name+" +>"+ data.Player.Pseudo);
		if(isContainRoom(ThisRoom.Name)){
			console.log(" "+ ThisRoom.Password + " == " + io.sockets.adapter.rooms[ThisRoom.Name].Password);
			if (ThisRoom.Password == io.sockets.adapter.rooms[ThisRoom.Name].Password){
				var rooms_players = io.sockets.adapter.rooms[ThisRoom.Name].Players;
				var i = 0;
				while(rooms_players[i] != undefined)
					i++;
				if (i>rooms_players.length){ 
					socket.emit("joinRoomEcho", "Error: Cette room étais déjà pleine.");
				}else{
					socket.join(ThisRoom.Name);
					roomResult = ThisRoom;
					roomResult.Players[i] = data.Player;
					socket.emit("joinRoomEcho", roomResult);
					socket.broadcast.to(ThisRoom.Name).emit('hostCheck', ThisRoom);	
				}
			}else
				socket.emit("joinRoomEcho", "Error: Ce mot de passe est faux.");
		}else
			socket.emit("joinRoomEcho", "Error: Cette room n'existe pas.");
	});	

	socket.on('lunchGame', room =>{
		socket.broadcast.to(room.Name).emit('lunchGame', room);
	});

	socket.on('SendMessage', msg =>{
		socket.broadcast.to(msg.RoomName).emit('ReceiveMessage', msg);
		socket.emit('ReceiveMessage', msg);
	});
	
	//#region base
	socket.on('add user', (username) => {
		//WriteConsole("\n->> AddUser\n"+username);
		if (addedUser) return;

		// we store the username in the socket session for this client
		socket.username = username;
		++numUsers;
		addedUser = true;
		socket.emit('login', {
			numUsers: numUsers
	});
		// echo globally (all clients) that a person has connected
		socket.broadcast.emit('user joined', {
			username: socket.username,
			numUsers: numUsers
		});
	});

	socket.on('typing', () => {
		socket.broadcast.emit('typing', {
			username: socket.username
		});
	});

	socket.on('stop typing', () => {
		socket.broadcast.emit('stop typing', {
			username: socket.username
		});
	});

	//#endregion
});

function ViewPlayers(players){
  return "players ("+players.length+") :"+players.map(function(p) {return p.pseudo; });  
}
function ViewRooms(rooms){
  return "rooms ("+rooms.length+") :"+rooms.map(function(r) {return r.Name; });  
}
function WriteConsole(message){
  console.log(Players.length+") "+message)
}
function isContainRoom(RoomName){
	for (let r = 0; r < Rooms.length; r++) {
		if (Rooms[r].Name == RoomName)
			return true;
	}
	return false
}

//for test
function dump(obj) {
    var out = '';
    for (var i in obj) {
        out += i + ": " + obj[i] + "\n";
    }
    console.log(out);
}