
namespace Content.Shared.ADT
{
    [RegisterComponent]
    public abstract class BaseEmitEmoteComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("emote", required: true)]
        //public EmotePrototype? Emote { get; set; }
        public string? EmoteType { get; set; }
    }
}
