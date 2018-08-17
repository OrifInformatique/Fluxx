<?php 
	require('connection.php');

//ce fichier vas chercher les joueurs correspondant à une room via son id.

	$idRoom = $_POST["room_id"];

	$query = "SELECT fk_host, fk_player_2, fk_player_3, fk_player_4, fk_player_5, fk_player_6 FROM room WHERE id = '$idRoom'";
	$result = implode('\',\'',mysqli_query($conn, $query)->fetch_assoc()); 
	$query = "SELECT id, pseudo, color, victory, defeat FROM player WHERE id IN ('$result')";
	$result2 = mysqli_query($conn, $query); 
	$i = 0;
	while($row = $result2->fetch_assoc()){
		$array[$i]['Id'] = $row['id'];
		$array[$i]['Pseudo'] = $row['pseudo'];
		$array[$i]['Color'] = $row['color'];
		$array[$i]['Wins'] = $row['victory'];
		$array[$i]['Losses'] = $row['defeat'];
		$i++;
	}

	
	echo json_encode($array);

mysqli_close($conn);
?>