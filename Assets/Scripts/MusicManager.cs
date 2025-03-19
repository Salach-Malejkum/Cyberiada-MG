using UnityEngine;
using System;
using System.Runtime.InteropServices;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField]
    public EventReference music;
    

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public event Action<int> OnBeatChanged;
        public int CurrentBeat 
        {
            get => _currentBeat;
            set 
            {
                if (_currentBeat != value)
                {
                    _currentBeat = value;
                    OnBeatChanged?.Invoke(_currentBeat);
                }
            }
        }
        private int _currentBeat = 0;
        public int currentBar = 0;
        public float currentTempo = 0;
        public int currentPosition = 0;
        public float songLength = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    public TimelineInfo timelineInfo = null;
    private GCHandle timelineHandle;

    private FMOD.Studio.EVENT_CALLBACK beatCallback;
    private FMOD.Studio.EventDescription descriptionCallback;

    public FMOD.Studio.EventInstance musicPlayEvent;


    private void Awake() {
        Instance = this;

        musicPlayEvent = RuntimeManager.CreateInstance(music);
        musicPlayEvent.start();
        timelineInfo = new TimelineInfo();
    }


    private void Start()
    {
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);

        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        musicPlayEvent.setUserData(GCHandle.ToIntPtr(timelineHandle));
        musicPlayEvent.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);

        musicPlayEvent.getDescription(out descriptionCallback);
        descriptionCallback.getLength(out int length);

        timelineInfo.songLength = length;
    }


    private void Update()
    {
        musicPlayEvent.getTimelinePosition(out timelineInfo.currentPosition);
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    private static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new(instancePtr);

        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero) // System(IntPtr)
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.CurrentBeat = parameter.beat;
                        timelineInfo.currentBar = parameter.bar;
                        timelineInfo.currentTempo = parameter.tempo;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                    break;
            }
        }

        return FMOD.RESULT.OK;
    }


    private void OnDestroy()
    {
        musicPlayEvent.setUserData(IntPtr.Zero);
        musicPlayEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicPlayEvent.release();
        timelineHandle.Free();
    }

    public void Subscribe(Action<int> onBeatChanged)
    {
        timelineInfo.OnBeatChanged += onBeatChanged;
    }


    public void Unsubscribe(Action<int> onBeatChanged)
    {
        timelineInfo.OnBeatChanged -= onBeatChanged;
    }
}