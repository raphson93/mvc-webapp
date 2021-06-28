function Home() {
    this.init = function (url) {
        $('#onLogout').click(function () {
            let user = localStorage.getItem("user");
            let authenticationBody = { User: user, JWToken: localStorage.getItem("JWToken") };
            let request = $.ajax({
                url: "/Login/Logout",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(authenticationBody)
            });

            request.done(function (data) {
                console.log(data);
                localStorage.clear();
                location.replace(data);
            });

            request.fail(function (data) {
                console.log(data);
            });
        });
    }
}