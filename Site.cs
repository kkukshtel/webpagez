namespace webpagez;

using System.Text;
public class SiteElements
{
    public record TagAttribute(string key, string value)
    {
        public string Build() => @$"{key}=""{value}""";
    }

    public record MultiValueTagAttribute(string key, List<string> values) :
        TagAttribute(key,string.Join(" ",values));

    public record Tag : IBuildable
    {
        public string Id;
        public string Name;
        public string I 
        {
            get => Inner;
            set => Inner = value;
        }
        public string Inner;
        public List<TagAttribute> Attrs;
        public List<IBuildable> T 
        {
            get => Tags;
            set => Tags = value;
        }
        public List<IBuildable> Tags;
        public Tag (string name, string id = "", string content = "",List<IBuildable> innerTags = null, List<TagAttribute> attrs = null)
        {
            Name = name;
            Inner = content; 
            Tags = innerTags;
            Attrs = attrs;
            Id = id;
        }
        public void Build(StringBuilder sb)
        {
            sb.Append(Open());
            sb.Append(Inner);
            if(Tags != null)
            {
                foreach (var t in Tags)
                {
                    t.Build(sb);
                }
            }
            sb.Append(Close());
        }
        public virtual string Open()
        {
            Attrs ??= new ();
            if(!string.IsNullOrEmpty(Id))
            {
                Attrs.Insert(0,new("id",Id));
            }
            var tags = string.Join(" ",Attrs.Select(x => x.Build()));
            return $"<{Name} {tags}>";
        }
        public string Close() => $"</{Name}>";
    }

    public record Site(List<IBuildable> Tags)
    {
        public string Build()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            foreach (var t in Tags)
            {
                t.Build(sb);
            }
            return sb.ToString();
        }
    }


    // public static Site BaseSite = new ([
    //     HTMLBase with { Tags = [
    //         Head with { Tags = [
    //             Title
    //         ]},
    //         Body with { Tags = [
    //             Div
    //         ]}
    //     ]}
    // ]);

    public static Tag HTMLBase = new("html");
    public static Tag Head = new("head");
    public static Tag Body = new("body");
    public static Tag Div = new("div");
    public static Tag Paragraph = new("p");
    public static Tag P => Paragraph;
    public static Tag Title = new ("title");
    public static Tag HTMX = new("script",attrs:[new("src","https://unpkg.com/htmx.org@1.9.10")]);
    public static Tag Tailwind = new("script",attrs:[new("src","https://cdn.tailwindcss.com")]);
    
    // hx-get="http://localhost:5000/your-post-endpoint" hx-trigger="every 5s" hx-swap="outerHTML"
    public static TagAttribute Get = new("hx-get","#");
    public static TagAttribute Trigger = new ("hx-trigger","");
    public static TagAttribute Select = new ("hx-select","");
    public static TagAttribute Target = new ("hx-target","");
    public static TagAttribute SwapInner = new("hx-swap","innerHTML");
    public static TagAttribute SwapOuter = new("hx-swap","outerHTML");
    // public static TagAttribute Target = new("hx-target","");

    // public static MultiValueTagAttribute Indicator = new("class",["htmx-indicator"]);

    public interface IBuildable
    {
        void Build(StringBuilder sb);
    }

    public record PollingDiv(string path) : IBuildable
    {
        public void Build(StringBuilder sb)
        {
            var rand = new Random();
            var idap = rand.Next();
            var d = Div with {
                Attrs = [
                    Get with {value = path},
                    Trigger with {value = "every 1s"},
                    SwapInner,
                    Target with {value = "#polling-container"+idap}
                ],
                T = [
                    Div with { 
                        Id = "polling-container"+idap,
                        T = [
                            Div with {
                                Id = "test",
                                Inner = "Waiting..."
                            }
                        ]
                    }
                ]
            };
            d.Build(sb);
        }
    }
}




