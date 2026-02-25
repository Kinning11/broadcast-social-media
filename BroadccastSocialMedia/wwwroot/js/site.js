
document.addEventListener("DOMContentLoaded", function () {
    console.log("hej");

    const cards = document.querySelectorAll(".broadcast-card");

    cards.forEach(card => {
        card.addEventListener("click", () => {
            card.style.boxShadow = "0 6px 18px rgba(0,0,0,0.15)";
        });
    });



        const likeButtons = document.querySelectorAll(".like-btn");

        likeButtons.forEach(button => {

            button.addEventListener("click", function () {

                const broadcastId = this.dataset.broadcastId;

                fetch("/Home/ToggleLike", {
                    method: "POST",
                    body: new URLSearchParams({
                        BroadcastId: broadcastId
                    })
                })
                    .then(response => response.json())
                    .then(data => {

                        button.classList.toggle("liked");

                        if (button.classList.contains("liked")) {
                            button.textContent = "🤍 Unlike";
                        } else {
                            button.textContent = "❤️ Like";
                        }

                    });
            });

        });


        const followButtons = document.querySelectorAll(".follow-button"); // returnerar en lista 

        followButtons.forEach(button => { // loopar genom varje knapp

            button.addEventListener("click", function () { // lyssnar på klick, när användaren klickar - kör funktionen

                const userId = this.dataset.userId; // hämta data-attribut, från HTMLen ((data-user-id blir dataset.userId))
                const card = this.closest(".recommended-user");

                fetch("/Users/Listen", { // URL till controller action
                    method: "POST", // matcha [HttpPost i controllern]
                    body: new URLSearchParams({
                        UserId: userId //UserId från controllern och userId från HTML
                    })
                })
                    .then(response => response.json()) // Ta emot svar (data-user-id)
                    .then(data => {

                        // Lägg till animation
                        card.classList.add("removing");

                        // Ta bort elementet efter animation
                        setTimeout(() => {
                            card.remove();
                        }, 400);

                    });

            });

        });



});







   
