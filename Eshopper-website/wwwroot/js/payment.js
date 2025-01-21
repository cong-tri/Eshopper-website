// Immediately Invoked Function Expression to avoid global scope pollution
(function ($) {
  "use strict";

  // Check if PaymentTimer already exists to avoid conflicts
  if (window.PaymentTimer) {
    console.warn("PaymentTimer already exists, skipping initialization");
    return;
  }

  // Create PaymentTimer namespace
  var PaymentTimer = {
    _timer: null,
    _timeLeft: 0,

    init: function () {
      const timerDisplay = document.getElementById("payment-timer");
      if (timerDisplay) {
        this.startTimer(900);
      }
    },

    startTimer: function (duration) {
      var self = this;
      this._timeLeft = duration;

      // Clear existing timer if any
      if (this._timer) {
        clearInterval(this._timer);
      }

      // Start new timer
      this._timer = setInterval(function () {
        self.updateTime();
      }, 1000);
    },

    updateTime: function () {
      if (this._timeLeft <= 0) {
        if (this._timer) {
          clearInterval(this._timer);
        }

        // Show alert before redirect
        if (typeof Swal !== "undefined") {
          Swal.fire({
            title: "Hết thời gian thanh toán",
            text: "Bạn sẽ được chuyển về trang giỏ hàng",
            icon: "warning",
            confirmButtonText: "OK",
          }).then(function () {
            window.location.href = "/Cart/Index";
          });
        } else {
          alert("Hết thời gian thanh toán");
          window.location.href = "/Cart/Index";
        }
        return;
      }

      var minutes = Math.floor(this._timeLeft / 60);
      var seconds = this._timeLeft % 60;

      // Update timer display
      var timerDisplay = document.getElementById("payment-timer");
      if (timerDisplay) {
        timerDisplay.textContent =
          minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
      }

      this._timeLeft--;
    },

    clearTimer: function () {
      if (this._timer) {
        clearInterval(this._timer);
        this._timer = null;
      }
    },
  };

  // Attach to window object
  window.PaymentTimer = PaymentTimer;

  // Initialize when document is ready
  $(document).ready(function () {
    PaymentTimer.init();
  });

  // Clear timer when leaving page
  $(window).on("beforeunload", function () {
    PaymentTimer.clearTimer();
  });
})(jQuery);
