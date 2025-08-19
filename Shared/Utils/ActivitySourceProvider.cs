using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Shared.Utils
{
    public static class ActivitySourceProvider
    {
        private static readonly ActivitySource _activitySource = new ActivitySource("Zosser");

        /// <summary>
        /// يبدأ activity جديد مع اسم الدالة تلقائياً
        /// </summary>
        /// <param name="name">اسم الـ activity - يتم تعيينه تلقائياً من اسم الدالة المستدعية</param>
        /// <returns>Activity object أو null</returns>
        public static Activity? StartActivity([CallerMemberName] string? name = null)
        {
            var activityName = name ?? "UnknownActivity";
            return _activitySource.StartActivity(activityName);
        }

        /// <summary>
        /// إنهاء الـ ActivitySource عند إغلاق التطبيق
        /// </summary>
        public static void Dispose()
        {
            _activitySource?.Dispose();
        }
    }
}
