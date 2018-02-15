/******************************************************************************
*
* Filename: PS4000Imports.cs
*  
* Description:
*  This file contains .NET wrapper calls corresponding to function calls 
*  defined in the ps4000Api.h C header file. 
*  It also has the enums and structs required by the (wrapped) function calls.
*  
*  It is based on PicoSDK 10.6.12.41
*   
* Copyright © 2009-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PS4000Imports
{
	class Imports
	{
		#region Constants
		private const string _DRIVER_FILENAME = "ps4000.dll";

		public const int MaxValue = 32764;
		#endregion

		#region Driver Enums

		public enum Channel : int
		{
			ChannelA,
			ChannelB,
			ChannelC,
			ChannelD,
			External,
			Aux,
			None,
		}

		public enum Range : int
		{
			Range_10MV,
			Range_20MV,
			Range_50MV,
			Range_100MV,
			Range_200MV,
			Range_500MV,
			Range_1V,
			Range_2V,
			Range_5V,
			Range_10V,
			Range_20V,
			Range_50V,
            Range_100V,
            Range_MAX,

            Resistance_100R = Range_MAX,
            Resistance_1k,
            Resistance_10k,
            Resistance_100k,
            Resistance_1M,
            PS4000_MAX_RESISTANCES,
            /* FIXME: More ranges here, for accelerometer, etc. */
        }

		public enum TimeUnits : int
		{
			FemtoSeconds,
			PicoSeconds,
			NanoSeconds,
			MicroSeconds,
			MilliSeconds,
			Seconds
        }

        public enum ThresholdMode : int
		{
			Level,
			Window
		}

		public enum ThresholdDirection : int
		{
			// Values for level threshold mode
			//
			Above,
			Below,
			Rising,
			Falling,
			RisingOrFalling, // using both threshold
            AboveLower,  // using lower threshold
            BelowLower, // using lower threshold
            RisingLower,       // using upper threshold
            FallingLower,     // using upper threshold

            // Values for window threshold mode
            //
            Inside = Above,
			Outside = Below,
			Enter = Rising,
			Exit = Falling,
			EnterOrExit = RisingOrFalling,
            PositiveRunt = 9,
            NegativeRunt,
			None = Rising,
		}

		public enum DownSamplingMode : int
		{
			None,
			Aggregate,
            Average
		}

		public enum PulseWidthType : int
		{
			None,
			LessThan,
			GreaterThan,
			InRange,
			OutOfRange
		}

		public enum TriggerState : int
		{
			DontCare,
			True,
			False,
		}

        public enum Model : int
        {
            NONE = 0,
            PS4223 = 4223,
            PS4224 = 4224,
            PS4423 = 4423,
            PS4424 = 4424,
            PS4226 = 4226,
            PS4227 = 4227,
            PS4262 = 4262,
        }

        public enum PicoInfo : UInt32
        {
            DRIVER_VERSION = 0,
            USB_VERSION = 1,
            HARDWARE_VERSION = 2,
            VARIANT_INFO = 3,
            BATCH_AND_SERIAL = 4,
            CAL_DATE = 5,
            KERNEL_VERSION = 6
        }

        public enum SweepType : int {
            UP = 0,
            DOWN,
            UPDOWN,
            DOWNUP,
            MAX_SWEEP_TYPES
        }
        public enum IndexMode : int {
            SINGLE = 0,
            DUAL,
            QUAD,
            MAX_INDEX_MODES
        }

        public enum SigGenTrigType : int {
            SIGGEN_RISING = 0,
            SIGGEN_FALLING,
            SIGGEN_GATE_HIGH,
            SIGGEN_GATE_LOW
        }

        public enum SigGenTrigSource : int {
            SIGGEN_NONE = 0,
            SIGGEN_SCOPE_TRIG,
            SIGGEN_AUX_IN,
            SIGGEN_EXT_IN,
            SIGGEN_SOFT_TRIG
        }

        public enum PS4000EtsMode : int {
            ETS_OFF = 0,             // ETS disabled
            ETS_FAST,
            ETS_SLOW,
            ETS_MODES_MAX
        }
        public enum Probe : int {
            P_NONE = 0,
            P_CURRENT_CLAMP_10A,
            P_CURRENT_CLAMP_1000A,
            P_TEMPERATURE_SENSOR,
            P_CURRENT_MEASURING_DEVICE,
            P_PRESSURE_SENSOR_50BAR,
            P_PRESSURE_SENSOR_5BAR,
            P_OPTICAL_SWITCH,
            P_UNKNOWN,
            P_MAX_PROBES = P_UNKNOWN
        }
        public enum HoldOffType : int {
            Time,
            PS4000_MAX_HOLDOFF_TYPE
        }

        public enum ChannelInfo : int {
            Ranges = 0,
            Resistances,
            Accelerometer,
            Probes,
            Temperatures
        }

        public enum FrequencyCounterRange : int {
            FC_2K = 0,
            FC_20K,
            FC_20,
            FC_200,
            FC_MAX
        }

        #endregion

        #region Driver Structs
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct TriggerChannelProperties
		{
			public short ThresholdMajor;
			public ushort HysteresisMajor;
			public short ThresholdMinor;
			public ushort HysteresisMinor;
			public Channel Channel;
			public ThresholdMode ThresholdMode;


			public TriggerChannelProperties(
				short thresholdMajor,
				ushort hysteresisMajor,
				short thresholdMinor,
				ushort hysteresisMinor,
				Channel channel,
				ThresholdMode thresholdMode)
			{
				this.ThresholdMajor = thresholdMajor;
				this.HysteresisMajor = hysteresisMajor;
				this.ThresholdMinor = thresholdMinor;
				this.HysteresisMinor = hysteresisMinor;
				this.Channel = channel;
				this.ThresholdMode = thresholdMode;
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct TriggerConditions
		{
			public TriggerState ChannelA;
			public TriggerState ChannelB;
			public TriggerState ChannelC;
			public TriggerState ChannelD;
			public TriggerState External;
			public TriggerState Aux;
			public TriggerState Pwq;

			public TriggerConditions(
				TriggerState channelA,
				TriggerState channelB,
				TriggerState channelC,
				TriggerState channelD,
				TriggerState external,
				TriggerState aux,
				TriggerState pwq)
			{
				this.ChannelA = channelA;
				this.ChannelB = channelB;
				this.ChannelC = channelC;
				this.ChannelD = channelD;
				this.External = external;
				this.Aux = aux;
				this.Pwq = pwq;
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PwqConditions
		{
			public TriggerState ChannelA;
			public TriggerState ChannelB;
			public TriggerState ChannelC;
			public TriggerState ChannelD;
			public TriggerState External;
			public TriggerState Aux;

			public PwqConditions(
				TriggerState channelA,
				TriggerState channelB,
				TriggerState channelC,
				TriggerState channelD,
				TriggerState external,
				TriggerState aux)
			{
				this.ChannelA = channelA;
				this.ChannelB = channelB;
				this.ChannelC = channelC;
				this.ChannelD = channelD;
				this.External = external;
				this.Aux = aux;
			}
		}

        #endregion

        #region Driver Imports
        #region Callback delegates
        public delegate void BlockReady(short handle, short status, IntPtr pVoid);

		public delegate void StreamingReady(
												short handle,
												int noOfSamples,
												uint startIndex,
												short ov,
												uint triggerAt,
												short triggered,
												short autoStop,
												IntPtr pVoid);

		public delegate void DataReady(
												short handle,
												int noOfSamples,
												short overflow,
												uint triggerAt,
												short triggered,
												IntPtr pVoid);
        #endregion

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000OpenUnit")]
        public static extern short OpenUnit(out short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000OpenUnitAsync")]
        public static extern short OpenUnitAsync(out short status);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000OpenUnitEx")]
        public static extern short OpenUnitEx(out short handle, out byte serial);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000OpenUnitAsyncEx")]
        public static extern short OpenUnitAsyncEx(out short status, out byte serial);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000OpenUnitProgress")]
        public static extern short OpenUnitProgress(out short handle, out short progressPercent, out short complete);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetUnitInfo")]
        public static extern short GetUnitInfo(short handle, StringBuilder infoString, short stringLength, out short requiredSize, PicoInfo info);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000FlashLed")]
        public static extern short FlashLed(short handle, short start);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000IsLedFlashing")]
        public static extern short IsLedFlashing(short handle, out short status);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000CloseUnit")]
		public static extern short CloseUnit(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000MemorySegments")]
        public static extern short MemorySegments(
            short handle,
            ushort nSegments,
            out int nMaxSamples);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetChannel")]
        public static extern short SetChannel(
            short handle,
            Channel channel,
            short enabled,
            short dc,
            Range range);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetNoOfCaptures")]
        public static extern short SetNoOfRapidCaptures(
            short handle,
            ushort nWaveforms);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetTimebase")]
        public static extern short GetTimebase(
            short handle,
            uint timebase,
            int noSamples,
            out int timeIntervalNanoseconds,
            short oversample,
            out int maxSamples,
            ushort segmentIndex);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetTimebase2")]
        public static extern short GetTimebase2(
            short handle,
            uint timebase,
            int noSamples,
            out float timeIntervalNanoseconds,
            short oversample,
            out int maxSamples,
            ushort segmentIndex);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SigGenOff")]
        public static extern short SigGenOff(short handle);


        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetSigGenArbitrary")]
        public static extern short SetSigGenArbitrary(
              short handle,
              Int32 offsetVoltage,
              UInt32 pkToPk,
              UInt32 startDeltaPhase,
              UInt32 stopDeltaPhase,
              UInt32 deltaPhaseIncrement,
              UInt32 dwellCount,
              short[] arbitraryWaveform,
              Int32 arbitraryWaveformSize,
              SweepType sweepType,
              short operationType,
              IndexMode indexMode,
              UInt32 shots,
              UInt32 sweeps,
              SigGenTrigType triggerType,
              SigGenTrigSource triggerSource,
              short extInThreshold
            );
        
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetSigGenBuiltIn")]
        public static extern short SetSigGenBuiltIn(
            short handle,
            Int32 offsetVoltage,
            UInt32 pkToPk,
            short waveType,
            float startFrequency,
            float stopFrequency,
            float increment,
            float dwellTime,
            SweepType sweepType,
            short operationType,
            UInt32 shots,
            UInt32 sweeps,
            SigGenTrigType triggerType,
            SigGenTrigSource triggerSource,
            short extInThreshold
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SigGenFrequencyToPhase")]
        public static extern short SigGenFrequencyToPhase(
            short handle,
            double frequency,
            IndexMode indexMode,
            UInt32 bufferLength,
            out UInt32 phase
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SigGenArbitraryMinMaxValues")]
        public static extern short SigGenArbitraryMinMaxValues(
            short handle,
            out short minArbitraryWaveformValue,
            out short maxArbitraryWaveformValue,
            out UInt32 minArbitraryWaveformSize,
            out UInt32 maxArbitraryWaveformSize
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SigGenSoftwareControl")]
        public static extern short SigGenSoftwareControl(short handle, short state);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetEts")]
        public static extern short SetEts(
              short handle,
              PS4000EtsMode mode,
              short etsCycles,
              short etsInterleave,
              out Int32 sampleTimePicoseconds
            );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetSimpleTrigger")]
        public static extern short SetSimpleTrigger(
            short handle,
            short enable,
            Channel source,
            short threshold,
            ThresholdDirection direction,
            UInt32 delay,
            short autoTrigger_ms
            );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetTriggerChannelProperties")]
        public static extern short SetTriggerChannelProperties(
            short handle,
            TriggerChannelProperties[] channelProperties,
            short numChannelProperties,
            short auxOutputEnable,
            int autoTriggerMilliseconds);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetExtTriggerRange")]
        public static extern short SetExtTriggerRange(
            short handle,
            Range extRange
            );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetTriggerChannelConditions")]
        public static extern short SetTriggerChannelConditions(
            short handle,
            TriggerConditions[] conditions,
            short nConditions
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetTriggerChannelDirections")]
        public static extern short SetTriggerChannelDirections(
                                        short handle,
                                        ThresholdDirection channelA,
                                        ThresholdDirection channelB,
                                        ThresholdDirection channelC,
                                        ThresholdDirection channelD,
                                        ThresholdDirection ext,
                                        ThresholdDirection aux);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetTriggerDelay")]
        public static extern short SetTriggerDelay(short handle, uint delay);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetPulseWidthQualifier")]
        public static extern short SetPulseWidthQualifier(
            short handle,
            PwqConditions[] conditions,
            short numConditions,
            ThresholdDirection direction,
            uint lower,
            uint upper,
            PulseWidthType type);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000IsTriggerOrPulseWidthQualifierEnabled")]
        public static extern short IsTriggerOrPulseWidthQualifierEnabled(
            short handle,
            out Int16 triggerEnabled,
            out Int16 pulseWidthQualifierEnabled
            );

        /* FIXME: These are unclear if timeUpper and timeLower are an array, or not, anyway one should probably just use the 64-bit version 

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetTriggerTimeOffset")]
        public static extern short GetTriggerTimeOffset(
            short handle,
            out UInt32 timeUpper,
            out UInt32 timeLower,
            enPS4000TimeUnits* timeUnits,
            uint16_t segmentIndex
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetTriggerChannelTimeOffset")]
        public static extern short GetTriggerChannelTimeOffset(
            int16_t handle,
            uint32_t* timeUpper,
            uint32_t* timeLower,
            PS4000_TIME_UNITS* timeUnits,
            uint16_t segmentIndex,
            PS4000_CHANNEL channel
        );
        */

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetTriggerTimeOffset64")]
        public static extern short GetTriggerTimeOffset64(
            short handle,
            out Int64 time,
            out TimeUnits timeUnits,
            UInt16 segmentIndex
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetTriggerChannelTimeOffset64")]
        public static extern short GetTriggerChannelTimeOffset64(
            short handle,
            out Int64 time,
            out TimeUnits timeUnits,
            UInt16 segmentIndex,
            Channel channel
        );
        /* FIXME: Skipped 32-bit definitions,as the 64-bit versions can be used, instead.
         * PREF0 PREF1 PICO_STATUS PREF2 PREF3 (ps4000GetValuesTriggerTimeOffsetBulk)
(
  int16_t            handle,
  uint32_t          *timesUpper,
  uint32_t          *timesLower,
  PS4000_TIME_UNITS *timeUnits,
  uint16_t           fromSegmentIndex,
  uint16_t           toSegmentIndex
);

PREF0 PREF1 PICO_STATUS PREF2 PREF3 (ps4000GetValuesTriggerChannelTimeOffsetBulk)
(
  int16_t            handle,
  uint32_t          *timesUpper,
  uint32_t          *timesLower,
  PS4000_TIME_UNITS *timeUnits,
  uint16_t           fromSegmentIndex,
  uint16_t           toSegmentIndex,
  PS4000_CHANNEL     channel
);
*/
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetValuesTriggerTimeOffsetBulk64")]
        public static extern short GetValuesTriggerTimeOffsetBulk64(
            short handle,
            Int64[] times,
            TimeUnits[] timeUnits,
            UInt16 fromSegmentIndex,
            UInt16 toSegmentIndex
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetValuesTriggerChannelTimeOffsetBulk64")]
        public static extern short GetValuesTriggerChannelTimeOffsetBulk64(
            short handle,
            Int64[] times,
            TimeUnits[] timeUnits,
            UInt16 fromSegmentIndex,
            UInt16 toSegmentIndex,
            Channel channel
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetDataBufferBulk")]
        public static extern short SetDataBufferBulk(
            short handle,
            Channel channel,
            Int16[] buffer,
            Int32 bufferLth,
            UInt16 waveform
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetDataBuffers")]
        public static extern short SetDataBuffers(
                                                short handle,
                                                Channel channel,
                                                short[] bufferMax,
                                                short[] bufferMin,
                                                int bufferLth);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetDataBufferWithMode")]
        public static extern short SetDataBufferWithMode(
                                                short handle,
                                                Channel channel,
                                                short[] buffer,
                                                int bufferLth,
                                                DownSamplingMode mode);


        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetDataBuffersWithMode")]
        public static extern short SetDataBuffersWithMode(
                                                short handle,
                                                Channel channel,
                                                short[] bufferMax,
                                                short[] bufferMin,
                                                int bufferLth,
                                                DownSamplingMode mode);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetDataBuffer")]
        public static extern short SetDataBuffer(
                                                short handle,
                                                Channel channel,
                                                short[] buffer,
                                                int bufferLth);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetEtsTimeBuffers")]
        public static extern short SetEtsTimeBuffers(
                                                short handle,
                                                UInt32[] timeUpper,
                                                UInt32[] timeLower,
                                                int bufferLth);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000RunBlock")]
        public static extern short RunBlock(
                                                short handle,
                                                int noOfPreTriggerSamples,
                                                int noOfPostTriggerSamples,
                                                uint timebase,
                                                short oversample,
                                                out int timeIndisposedMs,
                                                ushort segmentIndex,
                                                BlockReady lpps4000BlockReady,
                                                IntPtr pVoid);


        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000RunStreaming")]
        public static extern short RunStreaming(
                                                short handle,
                                                ref UInt32 sampleInterval,
                                                TimeUnits sampleIntervalTimeUnits,
                                                UInt32 maxPreTriggerSamples,
                                                UInt32 maxPostPreTriggerSamples,
                                                Int16 autoStop,
                                                UInt32 downSampleRatio,
                                                UInt32 overviewBufferSize
            );


        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000RunStreamingEx")]
        public static extern short RunStreamingEx(
                                                short handle,
                                                ref UInt32 sampleInterval,
                                                TimeUnits sampleIntervalTimeUnits,
                                                UInt32 maxPreTriggerSamples,
                                                UInt32 maxPostPreTriggerSamples,
                                                Int16 autoStop,
                                                UInt32 downSampleRatio,
                                                Int16 downSampleRatioMode,
                                                UInt32 overviewBufferSize
            );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000IsReady")]
        public static extern short IsReady(short handle, out Int16 ready);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetStreamingLatestValues")]
        public static extern short GetStreamingLatestValues(
            short handle,
            StreamingReady lpps4000StreamingReady,
            IntPtr pVoid);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000NoOfStreamingValues")]
        public static extern short NoOfStreamingValues(
            short handle,
            out UInt32 nCaptures);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetMaxDownSampleRatio")]
        public static extern short ps4000GetMaxDownSampleRatio(
            short handle,
            UInt32 noOfUnaggreatedSamples,
            out UInt32 maxDownSampleRatio,
            Int16 downSampleRatioMode,
            UInt16 segmentIndex
            );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetValues")]
        public static extern short GetValues(
                short handle,
                uint startIndex,
                ref uint noOfSamples,
                uint downSampleRatio,
                DownSamplingMode downSampleDownSamplingMode,
                ushort segmentIndex,
                out short overflow);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetValuesBulk")]
        public static extern short GetValuesRapid(
            short handle,
            ref uint noOfSamples,
            ushort fromSegmentIndex,
            ushort toSegmentIndex,
            short[] overflows);

        public delegate void StreamingReadyDelegate
        (
          Int16 handle,
          Int32 noOfSamples,
          UInt32 startIndex,
          Int16 overflow,
          UInt32 triggerAt,
          Int16 triggered,
          Int16 autoStop,
          IntPtr pParameter
        );

        // Callback is called from a separate thread
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetValuesAsync")]
        public static extern short GetValuesAsync(
            short handle,
            UInt32 startIndex,
            UInt32 noOfSamples,
            UInt32 downSampleRatio,
            Int16 downSampleRatioMode,
            UInt16 segmentIndex,
            StreamingReadyDelegate lpDataReady,
            IntPtr pParameter
        );
        public delegate void BlockReadyDelegate
        (
          short handle,
          Int32 noOfSamples,
          Int16 overflow,
          UInt32 triggerAt,
          Int16 triggered,
          IntPtr pParameter
        );
        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetValuesAsync")]
        public static extern short GetValuesAsync(
            short handle,
            UInt32 startIndex,
            UInt32 noOfSamples,
            UInt32 downSampleRatio,
            Int16 downSampleRatioMode,
            UInt16 segmentIndex,
            BlockReadyDelegate lpDataReady,
            IntPtr pParameter
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000Stop")]
        public static extern short Stop(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetProbe")]
        public static extern short SetProbe(
            short handle,
            Probe probe,
            Range range
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000HoldOff")]
        public static extern short HoldOff(
            short handle,
            UInt64 holdoff,
            HoldOffType type
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetProbe")]
        public static extern short GetProbe(
            short handle,
            out Probe probe
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetChannelInformation")]
        public static extern short GetChannelInformation(
            short handle,
            ChannelInfo info,
            Int32 probe,
            Int32[] ranges,
            ref Int32 length,
            Channel channels
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetFrequencyCounter")]
        public static extern short SetFrequencyCounter(
            short handle,
            Channel channel,
            Int16 enabled,
            FrequencyCounterRange range,
            Int16 thresholdMajor,
            Int16 thresholdMinor
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000EnumerateUnits")]
        public static extern short EnumerateUnits(
            out Int16 count,
            StringBuilder serials,
            ref Int16 serialLth
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000PingUnit")]
        public static extern short PingUnit(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetBwFilter")]
        public static extern short SetBwFilter(short handle, Channel channel, Int16 enable);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000TriggerWithinPreTriggerSamples")]
        public static extern short TriggerWithinPreTriggerSamples(short handle, Int16 state);


        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetNoOfCaptures")]
        public static extern short GetNoOfCaptures(
            short handle,
            out ushort nCaptures);
        #endregion
    }
}
