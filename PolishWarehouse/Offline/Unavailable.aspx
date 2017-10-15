<%

Response.Status = "503 Service Unavailable"
Response.CacheControl = "no-cache"
Response.AddHeader "Pragma", "no-cache"
Response.Expires = -1

%>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Unavailable</title>

    <link rel="shortcut icon" href="../Content/Images/favicon.ico" type="image/x-icon">
    <link rel="icon" href="../Content/Images/favicon.ico" type="image/x-icon">
    
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>

    <link href="../Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Content/Site.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <div class="container body-content">
        <br />
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3>Unavailable</h3>
                    </div>
                    <div class="panel-collapse" id="Polishcollapse">
                        <div class="panel-body text-center">
                            <div class="row">
                                <div class="col-lg-12">
                                    <h2>This cat is hard at work, updating the site. Come back later!</h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <img src="../Content/Images/503.jpg" class="error-image" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <hr />
        <footer></footer>
    </div>
</body>
</html>