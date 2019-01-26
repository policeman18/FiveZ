document.onreadystatechange = () => {
	if (document.readyState === "complete") {
		window.addEventListener("message", (event) => {
			console.log(`Type: ${event.data.type} | Name: ${event.data.name} | Data: ${event.data.data}`);
			switch(event.data.type) {

				// FiveZ Character
				case "fivez_character":
					switch(event.data.name) {
						case "OpenMenu":
							FiveZ_Character.OpenMenu(event.data.data);
                            break;
                        case "CloseMenu":
                            FiveZ_Character.CloseMenu(event.data.data);
                            break;
						case "UpdateCharacters":
							FiveZ_Character.UpdateCharacters(event.data.data);
						break;
						default:
							console.log(`Couldn't Find Event Name: ${event.data.name} for Type: ${event.data.type}`);
						break;
					}
				break;

				// Default Catcher
                default:
                    if (event.data.type != "message") {
						console.log(`Couldn't Find Event Type: ${event.data.type}`);
					}
				break;
			}
		});
	}
};