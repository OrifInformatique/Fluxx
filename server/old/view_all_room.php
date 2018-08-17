<?php 
	require('connection.php');

// ce fichier vas chercher toutes les rooms ouvertes.

	$query = "SELECT id, name, password, player_number, fk_host FROM room WHERE open = '0'";
	$result = mysqli_query($conn, $query); 
		
	$i = 0;
	while($row = $result->fetch_assoc()){
		$array[$i]['Id'] = $row['id'];
		$array[$i]['Name'] = $row['name'];
		$array[$i]['Password'] = $row['password'];
		$array[$i]['NumPlayer'] = $row['player_number'];
		$array[$i]['IdHost'] = $row['fk_host'];
		$i++;
	}
	
	echo json_encode($array);

mysqli_close($conn);
?>