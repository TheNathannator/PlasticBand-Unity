namespace PlasticBand.LowLevel
{
    // For devices that need special handling during domain reloads
    internal interface IDomainReloadReceiver
    {
        void OnDomainReload();
    }
}