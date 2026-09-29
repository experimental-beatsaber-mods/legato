#nullable enable

using System;

namespace Legato {
    internal static class BeatmapCharacteristicSelectionExtensions {
        internal static IDisposable SubscribeToCharacteristicSelection(this BeatmapCharacteristicSegmentedControlController controller, Action callback) =>
            new CharacteristicSelectionSubscription(controller, callback);
    }

    internal sealed class CharacteristicSelectionSubscription : IDisposable {
        private readonly BeatmapCharacteristicSegmentedControlController _controller;
        private readonly Action _callback;

        // didSelectBeatmapCharacteristicEvent's second parameter changed from BeatmapCharacteristicSO to a
        // plain BeatmapCharacteristic enum as of 1.45.1 (decompiled and confirmed against Main.dll) -- the
        // same rename already handled elsewhere in this project's other 1.45.1 ports (Heck, CustomJSONData).
#if BEAT_SABER_1_45_1
        private readonly Action<BeatmapCharacteristicSegmentedControlController, BeatmapCharacteristic> _handler;
#else
        private readonly Action<BeatmapCharacteristicSegmentedControlController, BeatmapCharacteristicSO> _handler;
#endif

        internal CharacteristicSelectionSubscription(BeatmapCharacteristicSegmentedControlController controller, Action callback) {
            _controller = controller;
            _callback = callback;
            _handler = (_, _) => _callback();
            _controller.didSelectBeatmapCharacteristicEvent += _handler;
        }

        public void Dispose() {
            _controller.didSelectBeatmapCharacteristicEvent -= _handler;
        }
    }
}
