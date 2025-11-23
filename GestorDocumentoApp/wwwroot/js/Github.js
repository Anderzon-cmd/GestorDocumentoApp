let repos = [];

document.addEventListener("DOMContentLoaded", () => {

    document.getElementById("ElementTypeId").addEventListener("change", async (event) => {
        const type = event.target.selectedOptions[0].text;
        
        const reposContainer = document.getElementById("reposContainer");
        const reposSelect = document.getElementById("reposSelect");

        const externalUrlElement = document.getElementById("ExternalUrlElement");
        const containerUrl = document.getElementById("containerUrl");


        if (type.toLowerCase() !== "codigo") {
            reposContainer.classList.add("hidden");
            reposSelect.innerHTML = ""

            externalUrlElement.value = "";
            containerUrl.classList.add("hidden");

            return;
        }
        containerUrl.classList.remove("hidden");
        reposContainer.classList.remove("hidden");

        reposSelect.innerHTML = "<option>Cargando...</option>"

        try {
            repos = await getAllRepositories();
            reposSelect.innerHTML = "<option>-- Seleccione un repositorio --</option>"
            repos.forEach(repo =>{
                const option = document.createElement("option");
                option.value = repo.id
                option.text = repo.name;
                reposSelect.appendChild(option);
            })

        } catch (error) {
            console.log(error);
            reposSelect.innerHTML = "<option>Error al cargar repositorios</option>";
        }
    });


    document.getElementById("reposContainer").addEventListener("change", (event) => {
        const code = event.target.value;
        
        const repo = repos.find(x => x.id === Number(code))
        console.log(repo)

        const textUrl = document.getElementById("ExternalUrlElement");
        
        if (!repo) {
            textUrl.value = "";
            return;
        }

        textUrl.value = repo.htmlUrl;


    })

     

    async function getAllRepositories() {
        try {
            const response = await fetch("/api/github/repos", {
                method: "GET",
                headers: {
                    "Accet": "application/json"
                }
            });

            if (!response.ok) {
                throw new Error("Error fetch respositories");
            }
            return await response.json()||[];
            
        } catch (error) {
            throw error;
        }
    }
    
});

