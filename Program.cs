using static webpagez.SiteElements;

var endpoint = "http://localhost:8080/";

Site BaseSite = new ([
    HTMLBase with { T = [
        Head with { T = [
            Title with { I = "webpagez" },
            Tailwind,
            HTMX
        ]},
        Body with { T = [
            new PollingDiv(endpoint + "poll/"),
            new PollingDiv(endpoint + "poll/"),
            new PollingDiv(endpoint + "poll/"),
            new PollingDiv(endpoint + "poll/")
        ]}
    ]}
]);

































File.WriteAllText("site.html",BaseSite.Build());
Console.WriteLine(BaseSite.Build());
// (BaseTooltip with {
//     header = "my new tooltip",
//     elements = new() {
//         value ? SmallImage : BodyText with { text = "alt text"},
//         BodyText with { text = "somebody text"},
//         SmallImage with { path = "another/image/path" }
//     }
// }).Build();