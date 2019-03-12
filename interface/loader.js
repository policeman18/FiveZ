const Pages = 
[
	"character"
]

async function LoadPage(page) {
	return new Promise(resolve => {
		axios.get(`./pages/${page}.html`, { headers: {"Content-Type" : "text/plain"} }).then((response) => {
			resolve(response.data);
		}).catch((error) => {
			resolve(null);
		});
	});
}

async function AddScript(page) {
	return new Promise(resolve => {
		let body = document.body;
		let newScript = document.createElement("script"); newScript.src = `./scripts/${page}.js`;
		body.append(newScript);
		resolve();
	});
}

async function AddPage(page, code) {
	return new Promise(resolve => {
		let container = document.getElementById("UI_Container");
		let newContainer = document.createElement("div"); newContainer.setAttribute("id", `${page}`);
		newContainer.style.position = "absolute"
		newContainer.style.width = "100%";
		newContainer.style.height = "100%";
		container.append(newContainer);
		newContainer.innerHTML = code;
		resolve();
	});
}

function StartListener() {
	let listenerScript = document.createElement("script");
	listenerScript.src = "./listener.js";
	document.body.append(listenerScript);
	axios.post("http://fivez/fivez_nui_loaded", {}).then((response) => {}).catch((error) => {});
}

document.onreadystatechange = async () => {
	if (document.readyState === "complete") {
		Pages.forEach(async page => {
			let pageCode = await LoadPage(page);
			if (pageCode != null) {
				await AddScript(page);
				await AddPage(page, pageCode);
				if (page == Pages[Pages.length - 1]) {
					StartListener();
				}
			}
		})
	}
}