// Solidity version
pragma solidity >=0.4.25 <0.6.0;

// Organ Transport App Smart Contract
contract OrganTransportApp
{
    // Donation state types
    enum StateType {
        Creating,
        Created,
        Ready,
        InTransit,
        Monitoring,
        Completed,
        Spoiled
    }

    // Donation state property
    StateType public DonationState;

    // Participating agents
    address public DonationCenter;
    address public OrganSender;
    address public TransportVehicle;
    address public TelemetryDevice;
    address public OrganReceiver;

    // Temperature threshold
    int public MinTemperature;
    int public MaxTemperature;

    // Real-time temperature reading property
    int public SensorTemperature;

    // Donated organ information property
    string public DonationInfo;

    // Organ Transport App Smart Contract constructor
    constructor(address sending, address vehicle, address device, address receiving, string memory donation, int min, int max) public
    {
        DonationCenter = msg.sender;
        OrganSender = sending;
        TransportVehicle = vehicle;
        TelemetryDevice = device;
        OrganReceiver = receiving;
        DonationInfo = donation;
        MinTemperature = min;
        MaxTemperature = max;
        if (MinTemperature >= MaxTemperature)
        {
            revert();
        }
        DonationState = StateType.Created;
    }

    // The organ is ready in the sending hospital.
    function OrganReady() public
    {
        if (OrganSender != msg.sender || DonationState != StateType.Created)
        {
            revert();
        }
        DonationState = StateType.Ready;
    }

    // The organ is in transit inside the transport vehicle.
    function OrganInTransit() public
    {
        if (TransportVehicle != msg.sender || DonationState != StateType.Ready)
        {
            revert();
        }
        DonationState = StateType.InTransit;
    }

    // The organ has arrived ot the receiving hospital.
    function OrganArrived() public
    {
        if (OrganReceiver != msg.sender || DonationState != StateType.Monitoring)
        {
            revert();
        }
        DonationState = StateType.Completed;
    }

    // The temperature is checked inside the organ transport fridge.
    function TemperatureCheck(int temperature) public
    {
        if (TelemetryDevice != msg.sender || (DonationState != StateType.InTransit && DonationState != StateType.Monitoring))
        {
            revert();
        }
        else
        {
            if (temperature > MaxTemperature || temperature < MinTemperature)
            {
                DonationState = StateType.Spoiled;
            }
            SensorTemperature = temperature;
            if (DonationState == StateType.InTransit)
            {
                DonationState = StateType.Monitoring;
            }
        }
    }

}