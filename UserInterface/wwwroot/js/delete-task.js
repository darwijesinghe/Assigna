// imports
import { Process, Confirm, Error, TaskCount } from "./helper-js.js";
import { Toast } from "./toast.js";

// delete task
const delButton = document.getElementById("delete-task");
const taskId = delButton && delButton.value;
const container = document.getElementById("view-content");
const remButton = document.getElementById("send-remind");

// hook event to button
delButton &&
  delButton.addEventListener("click", (e) => {
    try {
      Confirm.show(
        "Delete Confirmation",
        "Are you sure to delete this task?",
        (e) => {
          Process.show();
          fetch(`/task/deletetask?taskid=${taskId}`, {
            method: "POST",
            headers: {
              "X-Requested-With": "XMLHttpRequest",
            },
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
                let result = data.result;
                container.innerHTML = result;
                TaskCount(data.tasks.result);
                Toast.show(data.message);
              } else {
                Toast.show(data.message);
              }
            })
            .catch((error) => {
              Process.hide();
              console.log(error);
            });
        }
      );
    } catch (error) {
      console.log(error);
    }
  });

remButton.addEventListener("click", (e) => {
  try {
    let id = remButton.getAttribute("data-id");
    let name = remButton.getAttribute("data-name");
    let token = remButton.getAttribute("data-token");
    SendMail.show(id, name, token);
  } catch (error) {
    console.log(error);
  }
});

// send remind
const SendMail = {
  show(id, name, token) {
    try {
      const html = `<div class="reminder" id="reminder">
            <div class="rem_window">
                <div class="rem_title">
                    <p class="rem_topic">Send Email Reminder</p>
                    <div class="rem_close">
                        <span class="material-symbols-outlined icon" id="rem-close" role="button">
                            close
                        </span>
                    </div>
                </div>
                <div class="rem_body">
                    <form id="remind" asp-action="#?">
                        <div class="remind_to">
                            <span class="rem_to">To</span>
                            <span class="rem_name">${name}</span>
                            <input type="hidden" id="mail-to" value="${id}">
                        </div>
                        <div class="input_control">
                            <textarea id="mail-note" rows="5" role="textbox" placeholder="Write Remind Note"></textarea>
                            <span class="val_msg"></span>
                        </div>
                        <div class="rem_actions">
                            <button class="button" type="submit" role="button">Send</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>`;

      const template = document.createElement("template");
      template.innerHTML = html;

      // events
      const backDrop = template.content.getElementById("reminder");
      const close = template.content.getElementById("rem-close");
      const mailForm = template.content.getElementById("remind");
      const mailTo = template.content.getElementById("mail-to");
      const mailNote = template.content.getElementById("mail-note");

      backDrop.addEventListener("click", (e) => {
        if (e.target === backDrop) {
          this.hide(backDrop);
        }
      });

      close.addEventListener("click", (e) => {
        this.hide(backDrop);
      });

      // send remind
      mailForm &&
        mailForm.addEventListener("submit", (e) => {
          try {
            // bind form data to model
            const formData = new FormData();
            formData.append("task_id", mailTo.value);
            formData.append("message", mailNote.value.trim());

            e.preventDefault();
            if (!Validate()) {
              return false;
            }
            this.hide(backDrop);
            Process.show();
            fetch("/task/sendreminder", {
              method: "POST",
              headers: {
                "X-Requested-With": "XMLHttpRequest",
                RequestVerificationToken: token,
              },
              body: formData,
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
          if (mailNote.value.trim() === "") {
            Error.seterror(mailNote, "Email message can not be empty");
            return false;
          } else {
            Error.diserror(mailNote);
          }
          return true;
        } catch (error) {
          console.log(error);
        }
      }

      document.body.appendChild(template.content);
    } catch (error) {
      console.log(error);
    }
  },
  // hide remind
  hide(element) {
    try {
      document.body.removeChild(element);
    } catch (error) {
      console.log(error);
    }
  },
};
