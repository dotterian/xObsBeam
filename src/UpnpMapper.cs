
using SharpOpenNat;

namespace xObsBeam;

public class UpnpMapper
{
    // Сюда писать все подключенные маппинги
    List<int> mappings = new();
    IPHandler _iPHandler = new();
    NatDevice? device;

    public async Task<string> Map(int port, string? nickname)
    {
        var cts = new CancellationTokenSource(5000);
        device = await NatDiscoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
        var ip = await device.GetExternalIPAsync();
        await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, port, port, "OBS Beam"));
        await device.CreatePortMapAsync(new Mapping(Protocol.Udp, port, port, "OBS Beam"));
        mappings.Add(port);
        if (nickname != null) {
           await _iPHandler.WriteToStorage("rwz6gk0sea.execute-api.us-east-1.amazonaws.com", nickname, ip.ToString());
        }
        return string.Format("{0}:{1}", ip, port);
    }

    public async void Free()
    {
        if (device == null) {
            return;
        }
        foreach (int port in mappings) {
            await device.DeletePortMapAsync(new Mapping(Protocol.Tcp, port, port));
            await device.DeletePortMapAsync(new Mapping(Protocol.Udp, port, port));
        }
    }
}