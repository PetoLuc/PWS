<?php

try{
//resend data from garni arcus 1055 to MyPWS API
//https://github.com/buffy007/garni/blob/master/updateweatherstation.php
//https://thisinterestsme.com/sending-json-via-post-php/

$json = "{";
foreach ($_GET as  $key => $value) {
	if($key == 'action')
	{
		$value = 'updateraw';
	}
	if($key =='ID')
	{
	$ID = $value;
	}
	if($key == "PASSWORD")
	{
	$PASSWORD = $value;
	}
	if($key != 'action' and $key!= 'realtime' and  $key!= 'rtfreq' and $key!='ID' and $key!='PASSWORD' )
	{
		if($key=='dateutc')
		{
			$json .=  '"' . $key . '":"' . $value . '",';
		}
		else 
		{
			$json .=  '"' . $key . '":' . $value . ',';
		}
	}
}

try{
	//resend wunderground - temporally allowed, then disable a use in own server 
	//https://support.weather.com/s/article/PWS-Upload-Protocol?language=en_US
	$export = 'rtupdate.wunderground.com/weatherstation/updateweatherstation.php?'.$_SERVER['QUERY_STRING'];
	$ch=curl_init();
	curl_setopt($ch, CURLOPT_URL, $export);
	curl_exec($ch);
	curl_close($ch);
	}catch (Exception $e){}


try{
//send to my server
	$json = substr($json, 0, -1);
	$json.='}';
	$ch = curl_init('http://pws.laznici.sk/pws/'.$ID.'/'.$PASSWORD.'/weather');
	curl_setopt($ch, CURLOPT_POST, 1); 
	curl_setopt($ch, CURLOPT_POSTFIELDS, $json); 
	curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type: application/json'));  	
	curl_exec($ch);
	curl_close($ch);
	}catch (Exception $e){}
	}
	catch(Exception $e)
	{}
echo 'success';