<?php
	require('connection.php');

// à revoir...

	$pseudo = $_POST["player_pseudo"];
	$password = sha1($_POST["player_password"]."Fluxx");

	$query = "SELECT * From player WHERE pseudo = '$pseudo'";
	if(mysqli_query($conn, $query)->fetch_assoc().count()>0){
		echo "Ce pseudo est déjà utilisé!";
	}
	else
	{
		$query = "INSERT INTO player (pseudo, password, color, victory, defeat, inRoom) values('$pseudo', '$password', '000000', 0, 0, 0)";

		if (mysqli_query($conn, $query)) {

			echo mysqli_insert_id($conn); 
		}else{ 
			echo "error"; 
		}

	}
	
mysqli_close($conn);
?>