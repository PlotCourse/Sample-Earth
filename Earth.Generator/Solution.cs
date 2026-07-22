using PlotStax.Gen.Client;
using PlotStax.Gen.Client.Attributes;

namespace Earth.Generator;

public static class Solution
{
    public static SolutionBuilder GetSolutionBuilder()
    {
        var slnBuilder = new SolutionBuilder("Earth")
            .WithToken("SAMPLES_ONLY_TOKEN") // this free-use token works with small numbers of structures
            .WithOptions(o =>
            {
                o.AppHostDirectory = "Earth.AppHost";
                o.AppHostProjectFileName = "Earth.AppHost.csproj";
                o.AppHostAssemblyName = "Earth.AppHost";
            });

        return slnBuilder;
    }
}

[PsComponent(CSharpComponentType.Library)]
[PsComponentInWebApi<WebApiProject>(WebApiProject.Earth_Support)]
interface Ocean
{
    [PsBroadcast]
    interface OceanSurface
    {
        int Temperature { get; }

        void NewHurricane(HurricaneCoordinate location);
    }

    [PsService]
    [PsServiceReferencesBroadcast<OceanSurface>]
    [PsServiceAutoGenMethodsForProfile<WaterDataProfile>(AutoGenMethod.All)]
    interface Water
    {
        Task UpdateWaterState();
    }
}

[PsComponent(CSharpComponentType.Library)]
[PsComponentInWebApi<WebApiProject>(WebApiProject.Earth_Main)]
interface Dolphin
{
    [PsService(ExposureSetting = SubComponentExposureSetting.AlwaysExternalWebApi)]
    [PsServiceReferencesService<Ocean.Water>]
    interface Tricks
    {
        Task<DolphinCoordinate[]> Backflip();
    }

    [PsInternalSingleton]
    [PsInternalSingletonReferencesBroadcast<Ocean.OceanSurface>(ReferenceType = BroadcastReferenceType.Subscribes)]
    interface MonitorOcean
    {
    }
}

[PsUiComponent]
[PsUiComponentReferences<Ocean>]
[PsUiComponentReferences<Dolphin>]
interface DolphinUi
{
    [PsUiElement]
    [PsUiElementReferencesService<Dolphin.Tricks>]
    [PsUiElementReferencesBroadcast<Ocean.OceanSurface>]
    interface EarthOcean
    {
    }
}

[PsRecord]
[PsRecordInDataProfile<WaterDataProfile>]
interface DolphinCoordinate
{
    int DolphinCoordinateId { get; }
    int X { get; }
    int Y { get; }
}

[PsRecord]
[PsRecordInDataProfile<WaterDataProfile>]
interface HurricaneCoordinate
{
    int HurricaneCoordinateId { get; }
    int X { get; }
    int Y { get; }
}


[PsDataProfile]
interface WaterDataProfile
{
}
