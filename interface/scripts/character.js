const Genders = {
	0: "Male",
	1: "Female"
}

const FiveZ_Character = new Vue({
	el: "#FiveZ_Character",

	data: {
		ResourceName: "fivez",
        ShowMenu: false,
        ShowCreator: false,
		Updating: false,
        Characters: [
            //{ FirstName: "Xander", LastName: "Harrison", Gender: 0 }
        ],
        NewCharacterValid: false,
        NewCharacterData: { FirstName: "", LastName: "", Gender: 0 },
        NewCharacterFormRules: {
            NameRules: [
                v => !!v || "Name is required",
                v => (v && v.length <= 20) || "Name must be less than 10 characters",
                v => (v && v.length > 2) || "Name must be greater than 2 characters"
            ],
            GenderRules: [
                v => !!v || "Gender is required"
            ]
        }
	},

	methods: {
		OpenMenu(chars) {
			this.ShowMenu = true;
			this.Characters = JSON.parse(chars);
		},

        CloseMenu() {
            this.ShowMenu = false;
        },

		CreateCharacter() {
            if (this.$refs.creatorform.validate()) {
                axios.post(`http://${this.ResourceName}/fivez_character_createcharacter`, {
                    first: this.NewCharacterData.FirstName,
                    last: this.NewCharacterData.LastName,
                    gender: this.NewCharacterData.Gender
                }).then((response) => { }).catch((error) => { console.log(error); });
                this.Updating = true;
                this.CloseCreator();
            }
        },

        CloseCreator() {
            this.ShowCreator = false;
            this.$refs.creatorform.reset();
        },

		SelectCharacter(_id) {
            axios.post(`http://${this.ResourceName}/fivez_character_selectcharacter`, { id: _id }).then((response) => { }).catch((error) => { console.log(error) });
            this.CloseCreator();
        },

		DeleteCharacter(_id) {
			axios.post(`http://${this.ResourceName}/fivez_character_deletecharacter`, {id: _id}).then((response) => {}).catch((error) => { console.log(error) });
            this.Updating = true;
        },

		UpdateCharacters(chars) {
			this.Characters = JSON.parse(chars);
			this.Updating = false;
		}
	}
})