<?php
	require('connection.php');

// ce fichier est appelé avec un pseudo et un mot de passe en paramètre et retourne toutes les info du joueurs a qui elle appartient si elle sont correctes.

	$pseudo = $_POST["player_pseudo"];
	$password = $_POST["player_password"];
	
	$query = "SELECT * FROM player WHERE pseudo = '$pseudo' LIMIT 1";
 	$result = mysqli_query($conn, $query)->fetch_assoc(); 

	$array['Id'] = $result['id'];
	$array['Pseudo'] = $result['pseudo'];
	$array['Color'] = $result['color'];
	$array['Password'] = $result['password'];
	$array['Wins'] = $result['victory'];
	$array['Losses'] = $result['defeat'];
	
	if($array != null && count($array) != 0){
		if ($array['Password'] == $password){
			echo json_encode($array);
		}
	}
	//si le mot de passe est faux ou que le pseudo ne correspond à rien, rien est retourné
mysqli_close($conn);
?>