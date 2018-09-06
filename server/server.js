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

var Players = [];//player_id, socket_id, pseudo, room_id
var Rooms = [];  //id, name, password, players

con.connect(function(err) {
	if (err)
		throw err;
	server.listen(port, () => {
		console.log('Server listening at port %d', port);
	});
	app.use(express.static(path.join(__dirname, 'public')));

	io.on('connection', (socket) => {
		WriteConsole("connection +");
		socket.on('playerConnection', player =>{
			socket.username = player.Pseudo;      
			Players.push({player_id:player.Id, socket_id:socket.id, pseudo:player.Pseudo, room_id:null});
			WriteConsole("connection : "+player.Pseudo);
			socket.emit('playerConnection', true);
		});
		socket.on('disconnect', () => {
			ViewPlayers(Players);
			console.log(" - "+ socket.username);
			for (let i = Players.length-1; i >= 0 ; i--) 
				if(Players[i].socket_id == socket.id) {
					if (Players[i].room != null)
						socket.broadcast.to(Players[i].room_id).emit('player leave room', Players[i].player_id);	
					Players.splice(i, 1);
				}  
			ViewPlayers(Players);
		});

		socket.on('loginPlayer', (player) => {
			con.query("SELECT * FROM `"+table+"` WHERE pseudo = \""+player.pseudo+"\" LIMIT 1", function (err, result, fields) {
				if (err) 
					throw err;
				var return_value = "Error: error";
				if (result[0]!=undefined)
					if (result[0].Password == player.password)
					{
						return_value = result[0];
						WriteConsole("connection db : "+result[0].Pseudo);
						for (let i = Players.length-1; i >= 0; i--) 
							if(socket.id == Players[i].socket_id)
							{
								ViewPlayers(Players);
								console.log(socket.username+" > "+result[0].Pseudo);
								socket.username = result[0].Pseudo;
								for (let i = Players.length-1; i >= 0 ; i--) 
									if(Players[i].socket_id == socket.id) 
										Players.splice(i, 1);
							}
						Players.push({player_id:result[0].Id, socket_id:socket.id, pseudo:result[0].Pseudo, room_id:null});
						ViewPlayers(Players);
					} 
					else
						return_value = "Error: Le mot de passe est incorrecte";
				else
					return_value = "Error: Ce pseudo n'existe pas";
				socket.emit("loginPlayerEcho", return_value);
			}); 
		});

		socket.on('createRoom', room =>{
		WriteConsole("\n->> createRoom:");
		console.log(room.Name);
		//console.log(room);//console.log(io.sockets.adapter.rooms);
		if(!Rooms.includes(room)){
			socket.join(room.Name);//first join
			Rooms.push(room);
			io.sockets.adapter.rooms[room.Name] = room;
			socket.emit("createRoomEcho", true);
		}
		else
			socket.emit("createRoomEcho", "Error: Une room porte déjà ce nom.");
		});

		socket.on('getAllRoom', function(){
		WriteConsole("\n->> getAllRoom:\n"+"Rooms[].name");//dsfaésdfizgalduigfliae
		var result = [];
		_rooms_name.forEach(room_name => {
			var room = [];
			room = io.sockets.adapter.rooms[room_name];
			console.log(io.sockets.adapter.rooms[room_name]);
			if(room!=null) 
				result.push(room); 
		});
		socket.emit("getAllRoomEcho", result);
		});
		
		socket.on('joinRoom', room =>{
		console.log("->> joinRoom");
		console.log(room);
		if(!(socket.rooms.indexOf(room.Name) >= 0)){
			if (io.socket.adapter.rooms[room.Name].Open){
				if (room.Password == io.socket.adapter.rooms[room.Name].Password){
					var rooms_players = io.socket.adapter.rooms[room.Name].Players;
					var i = 0;
					while(rooms_players[i] != undefined)
						i++;
					if (i>rooms_players.length){ 
						socket.emit("joinRoomEcho", "Error: Cette room étais déjà pleine.");
						io.socket.adapter.rooms[room.Name].Open = false;
					}else{
						socket.emit("joinRoomEcho", room);
						io.socket.adapter.rooms[room.Name].Players = player
						while(rooms_players[i] != undefined)
							i++;
						if(i>rooms_players.length)
							io.socket.adapter.rooms[room.Name].Open = false;
					}
				}else
					socket.emit("joinRoomEcho", "Error: Ce mot de passe set faux.");
			}else
			socket.emit("joinRoomEcho", "Error: Cette room est fermée.");
		}else
			socket.emit("joinRoomEcho", "Error: Cette room n'existe pas.");
		});	
		//#region base
		socket.on('add user', (username) => {
		//console.log("->> AddUser\n"+username);
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
});
function ViewPlayers(players){
  console.log("players ("+players.length+") :"+players.map(function(p) {return p.pseudo; }));  
}
function ViewRooms(rooms){
  console.log("rooms ("+rooms.length+") :"+rooms.map(function(r) {return r.name; }));  
}
function WriteConsole(message){
  console.log(Players.length+") "+message)
}