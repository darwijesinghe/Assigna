// imports
import { Error, Process, TaskCount } from "./helper-js.js";
import { Toast } from "./toast.js";

// edit task
const tskForm = document.getElementById("task");
const title = document.getElementById("tsk-title");
const category = document.getElementById("tsk-category");
const categoryParent = document.getElementById("tsk-cate-parent");
const deadLine = document.getElementById("tsk-date");
const oldDeadline = document.getElementById("tsk-date").value;
const deadLineParent = document.getElementById("tsk-date-parent");
const assignTo = document.getElementById("tsk-assign");
const assignToParent = document.getElementById("tsk-assign-parent");
const tskNote = document.getElementById("tsk-note");

// submission
tskForm.addEventListener("submit", (e) => {
  try {
    e.preventDefault();
    if (!Validate()) {
      return false;
    }
    Process.show();
    fetch("/task/edittask", {
      method: "POST",
      headers: {
        "X-Requested-With": "XMLHttpRequest",
      },
      body: new FormData(tskForm),
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
  } catch (error) {
    console.log(error);
  }
});

// validate
const options = {
  year: "numeric",
  month: "2-digit",
  day: "2-digit",
};

function Validate() {
  try {
    if (title.value.trim() === "") {
      Error.seterror(title, "Task title can not be empty");
      return false;
    } else {
      Error.diserror(title);
    }
    if (category.value.trim() === "") {
      Error.seterror(categoryParent, "Task category can not be empty");
      return false;
    } else {
      Error.diserror(categoryParent);
    }
    if (deadLine.value.trim() === "") {
      Error.seterror(deadLineParent, "Task date can not be empty");
      return false;
    } else {
      let oldValue = oldDeadline;
      let split = oldValue.split("-");
      let reformat = split[1] + "/" + split[2] + "/" + split[0];
      oldValue = new Date(reformat).toLocaleDateString("en-US", options);

      let pickDate = deadLine.value;
      split = pickDate.split("-");
      reformat = split[1] + "/" + split[2] + "/" + split[0];
      pickDate = new Date(reformat).toLocaleDateString("en-US", options);

      var dateRegex = /^\d\d\/\d\d\/\d\d\d\d$/;
      if (!dateRegex.test(pickDate)) {
        Error.seterror(deadLineParent, "Due date format is not valid");
        return false;
      } else {
        Error.diserror(deadLineParent);
      }

      if (pickDate < oldValue) {
        Error.seterror(deadLineParent, "Due date should not be old date");
        return false;
      } else {
        Error.diserror(deadLineParent);
      }
    }
    if (assignTo.value.trim() === "") {
      Error.seterror(assignToParent, "Task assignee can not be empty");
      return false;
    } else {
      Error.diserror(assignToParent);
    }
    if (tskNote.value.trim() === "") {
      Error.seterror(tskNote, "Task note can not be empty");
      return false;
    } else {
      Error.diserror(tskNote);
    }
    return true;
  } catch (error) {
    console.log(Error);
  }
}
