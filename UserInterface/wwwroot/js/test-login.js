// imports
import { Alert, Process, AlertType } from "./helper-js.js";

// test signin
const testLead = document.getElementById("te-lead");
const testMember = document.getElementById("te-member");

// submission
testLead.addEventListener("click", (e) => {
  try {
    Process.show();

    // data binds
    const userName = "manudi@lead";
    const password = "team@lead123";
    const token = testLead.getAttribute("data-token");

    const formData = new FormData();
    formData.append("UserName", userName);
    formData.append("Password", password);

    fetch("/account/signin", {
      method: "POST",
      headers: {
        "X-Requested-With": "XMLHttpRequest",
        RequestVerificationToken: token,
      },
      body: formData,
    })
      .then((response) => {
        Process.hide();
        return response.json();
      })
      .then((data) => {
        if (data.success) {
          var directlink = data.returnurl;
          return window.location.replace(directlink);
        } else {
          Alert.show(AlertType.error, data.message, "alert-message");
        }
      })
      .catch((error) => {
        Process.hide();
        console.log(error);
      });
  } catch (error) {
    Process.hide();
    console.log(error);
  }
});

testMember.addEventListener("click", (e) => {
  try {
    Process.show();

    // data binds
    const userName = "peter@work";
    const password = "team@member123";
    const token = testMember.getAttribute("data-token");

    const formData = new FormData();
    formData.append("UserName", userName);
    formData.append("Password", password);

    fetch("/account/signin", {
      method: "POST",
      headers: {
        "X-Requested-With": "XMLHttpRequest",
        RequestVerificationToken: token,
      },
      body: formData,
    })
      .then((response) => {
        Process.hide();
        return response.json();
      })
      .then((data) => {
        if (data.success) {
          var directlink = data.returnurl;
          return window.location.replace(directlink);
        } else {
          Alert.show(AlertType.error, data.message, "alert-message");
        }
      })
      .catch((error) => {
        Process.hide();
        console.log(error);
      });
  } catch (error) {
    Process.hide();
    console.log(error);
  }
});
