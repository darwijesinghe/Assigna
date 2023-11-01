const overlay = document.getElementById("nav_overlay");
const navContent = document.getElementById("nav_content");
const toggle = document.getElementById("header-toggle");
toggle &&
  toggle.addEventListener("click", () => {
    overlay.classList.toggle("nav_visible");
    navContent.classList.toggle("show_navigate");
  });

// Let the document know when the mouse is being used
document.body.addEventListener("mousedown", function () {
  document.body.classList.add("used-mouse");
});

// Re-enable focus styling when Tab is pressed
document.body.addEventListener("keydown", function (event) {
  if (event.keyCode === 9) {
    document.body.classList.remove("used-mouse");
  }
});

// remind modal
// document.getElementById("send-remind").addEventListener("click", (e) => {
//   document.getElementById("reminder").classList.toggle("rem_visible");
// });

var date_regex = /^\d\d\d\d\/\d\d\/\d\d$/;
if (!date_regex.test("2022/05/10")) {
  console.log("not valid");
} else {
  console.log("valid");
}
