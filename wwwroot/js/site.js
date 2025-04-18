// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    $('#btnGenerateOtp').off('click').on('click', function (e) {
        e.preventDefault();
        $('.text-danger').text('');
        if (!validateForm()) return;

        $('#btnGenerateOtp').prop('disabled', true);
        $('#otpLoading').show();

        var fullName = $('#txtFullName').val();
        var emailId = $('#txtEmail').val();
        var url = "/api/OtpApi/GenerateOtp/";        

        $.ajax({
            url: url,
            data: {
                fullName: fullName,
                emailId: emailId
            },
            dataType: "json",
            method: "POST",
            cache: false,
            success: function (response) {
                $('#btnGenerateOtp').prop('disabled', false);
                $('#otpLoading').hide();

                if (response) {
                    $('#otpVerificationSection').fadeIn();

                    $('#otpModalBody').text("OTP sent successfully to " + emailId);
                    var modal = new bootstrap.Modal(document.getElementById('otpModal'));
                    modal.show();
                } else {
                    $('#otpModalBody').text("Failed to send OTP. Please try again.");
                    var modal = new bootstrap.Modal(document.getElementById('otpModal'));
                    modal.show();
                }
            },
            error:function(err) {
                console.error(err);
                $('#btnGenerateOtp').prop('disabled', false);
                $('#otpLoading').hide();

                $('#otpModalBody').text("Failed to send OTP. Please try again.");
                var modal = new bootstrap.Modal(document.getElementById('otpModal'));
                modal.show();
            }
        });

    });

    $('#btnVerifyOtp').off('click').on('click', function (e) {
        var otp = $('#txtGeneratedOtp').val();
        var emailId = $('#txtEmail').val();
        var url = "/api/OtpApi/VerifyOtp/";
        $.ajax({
            url: url,
            data: {
                otp: otp,
                emailId: emailId
            },
            dataType: "json",
            method: "POST",
            cache: false,
            success: function (response) {
                if (response) {
                    $('#otpModalBody').text("OTP Verified Successfully ");
                    var modal = new bootstrap.Modal(document.getElementById('otpModal'));
                    modal.show();
                } else {
                    $('#otpModalBody').text("Invalid OTP ");
                    var modal = new bootstrap.Modal(document.getElementById('otpModal'));
                    modal.show();
                }
            },
            error: function (err) {
                console.error(err);
            }
        });
        
    });

    $('#otpModal').on('hidden.bs.modal', function () {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    });
    function isEmailValid(email) {
        var regex = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);
        return regex.test(email);
    }

    function isValidCountryCode(code) {
        const regex = /^\+?\d{1,4}$/;
        return regex.test(code);
    }
    function isValidPhoneNumber(phone) {        
        const regex = /^\d{8,10}$/;
        return regex.test(phone);
    }

    function validateForm() {
        let isValid = true;
        var fullName = $('#txtFullName').val().trim();
        var email = $('#txtEmail').val().trim();
        var countryCode = $('#txtCountryCode').val().trim();
        var phone = $('#txtPhNo').val().trim();

        if (!fullName) {
            $('#errorFullName').text('Full Name is required.');
            isValid = false;
        }

        if (!email) {
            $('#errorEmail').text('Email is required.');
            isValid = false;
        } else if (!isEmailValid(email)) {
            $('#errorEmail').text('Please enter a valid email address.');
            isValid = false;
        }

        if (!countryCode) {
            $('#errorCountryCode').text('Country Code is required.');
            isValid = false;
        } else if (!isValidCountryCode(countryCode)) {
            $('#errorCountryCode').text('Invalid Country Code.');
            isValid = false;
        }

        if (!phone) {
            $('#errorPhone').text('Phone Number is required.');
            isValid = false;
        } else if (!isValidPhoneNumber(phone)) {
            $('#errorPhone').text('Invalid Phone Number.');
            isValid = false;
        }

        return isValid;
    }

});
