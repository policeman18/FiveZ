const Genders = {
	0: "Male",
	1: "Female"
}

const FiveZ_Character = new Vue({
	el: "#FiveZ_Character",

	data: {
		ResourceName: "fivez",
		ShowMenu: true,
		Updating: false,
        Characters: [
            //{ FirstName: "Xander", LastName: "Harrison", Gender: 0 }
        ]
	},

	methods: {
		OpenMenu(chars) {
			this.ShowMenu = true;
			this.Characters = JSON.parse(chars);
		},

		CreateCharacter() {
			
		},

		SelectCharacter() {

		},

		DeleteCharacter(_id) {
			axios.post(`http://${this.ResourceName}/fivez_character_deletecharacter`, {id: _id}).then((response) => {
				this.Updating = true;
			}).catch((error) => { console.log(error) });
		},

		UpdateCharacters(chars) {
			this.Characters = JSON.parse(chars);
			this.Updating = false;
		}
	}
})