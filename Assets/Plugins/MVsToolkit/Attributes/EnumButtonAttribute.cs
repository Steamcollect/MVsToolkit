using UnityEngine;

namespace MVsToolkit.Dev
{
    /// <summary>
    /// Displays an enum as a row of clickable buttons in the Inspector.
    /// <para>
    /// Each enum value is drawn as a toolbar-style button, similar to the tab buttons
    /// used in the custom MVs inspector. Only one value can be selected at a time.
    /// </para>
    ///
    /// <para>
    /// Use <see cref="EnumButtonAttribute"/> to make enum selection faster and more visual,
    /// especially for mode switches, tool states, or gameplay options.
    /// </para>
    ///
    /// <para>Examples:</para>
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       <code>[EnumButton]</code> → Draws only the buttons, hiding the variable label.
    ///     </description>
    ///   </item>
    ///
    ///   <item>
    ///     <description>
    ///       <code>[EnumButton(true)]</code> → Explicitly shows the variable name.
    ///     </description>
    ///   </item>
    /// </list>
    /// </summary>
    public class EnumButtonAttribute : PropertyAttribute
    {
        /// <summary>
        /// If true, the variable name is displayed before the buttons.
        /// If false, only the buttons are shown.
        /// </summary>
        public readonly bool ShowVariableName;

        /// <param name="showName">
        /// Whether to display the variable name in the Inspector.
        /// <para>Default: false (only buttons are shown).</para>
        /// </param>
        public EnumButtonAttribute(bool showName = false)
        {
            ShowVariableName = showName;
        }
    }
}
