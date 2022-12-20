// imports
import { Error, Process } from "./helper-js.js";
import { Toast } from "./toast.js";

// add task note
const noteForm = document.getElementById("user-note");
const userNote = document.getElementById("as-note");
const newNote = document.getElementById("new-note");

// submission
noteForm.addEventListener("submit", (e) => {
  try {
    e.preventDefault();
    if (!Validate()) {
      return false;
    }
    Process.show();
    fetch("/task/writenote", {
      method: "POST",
      headers: {
        "X-Requested-With": "XMLHttpRequest",
      },
      body: new FormData(noteForm),
    })
      .then((response) => {
        Process.hide();
        if (response.status === 200) {
          return response.json();
        }
        if (response.status === 401) {
          var directlink = "/signin";
          return window.location.replace(directlink);
        }
      })
      .then((data) => {
        if (data.success) {
          noteForm.reset();
          newNote.textContent = data.result;
          Toast.show(data.message);
        } else {
          Toast.show(data.message);
        }
      })
      .catch((error) => {
        Process.hide();
        console.log(error);
      });
  } catch (error) {
    console.log(error);
  }
});

// validate
function Validate() {
  try {
    if (userNote.value.trim() === "") {
      Error.seterror(userNote, "Assignee note can not be empty");
      return false;
    } else {
      Error.diserror(userNote);
    }
    return true;
  } catch (error) {
    console.log(error);
  }
}
