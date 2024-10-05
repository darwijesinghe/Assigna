// toast message
export const Toast = {
    init() {
        // initializing
        try {
            this.cleantimeout = null;
            this.element = document.createElement("div");
            this.element.className = "toast";
            this.element.id = "toast";
            this.element.role = "alert";
            this.element.innerHTML = `<div class="to_content">
                                    <span id="t-message"></span>
                                    <div class="ba_close" id="t-close">
                                        <span class="material-symbols-outlined icon">close</span>
                                    </div>
                                </div>`;
            document.body.appendChild(this.element);

            // hook close button event
            document.getElementById("t-close").addEventListener("click", () => {
                document.getElementById("toast").classList.remove("to_visible");
            });
        } catch (error) {
            console.log(error);
        }
    },

    show(message) {
        try {
            // show toast
            clearTimeout(this.cleantimeout);
            document.getElementById("t-message").textContent = message;
            this.element.className = "toast to_visible";

            // hide toast
            this.cleantimeout = setTimeout(() => {
                this.element.classList.remove("to_visible");
            }, 5000);
        } catch (error) {
            console.log(error);
        }
    },
};

// initialize methods
document.addEventListener("DOMContentLoaded", () => {
    try {
        // toast
        Toast.init();
    } catch (error) {
        console.log(error);
    }
});
