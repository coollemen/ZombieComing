// --------------------------------------------------------------------------------------------------------------------
// Data Bind for Unity
// Copyright (c) Slash Games. All rights reserved.
// --------------------------------------------------------------------------------------------------------------------

Setup
-----

- Import DataBind.unitypackage into your project (you probably already did if you read this file).

- If you are using NGUI, double click on the NGUI.unitypackage in Slash.Unity.DataBind/Addons to extract the NGUI specific scripts.

- If it's the first time you are using Data Bind for Unity:
-- Check the examples in DataBind/Examples
-- Read through the documentation at https://bitbucket.org/coeing/data-bind/wiki
-- The API of the classes is documented at http://slashgames.org/tools/data-bind/api

- If you encounter any issues (bugs, missing features,...) please create a new issue at the official issue tracker at https://bitbucket.org/coeing/data-bind/issues

- Any feedback, positive as well as negative, is always appreciated at contact@slashgames.org or at the official Unity forum thread at http://forum.unity3d.com/threads/released-data-bind-for-unity.298471/ Thanks for your help!

Changelog
---------

Version 1.0.7 (10/02/2017)
**************************

* Add event ClearedItems to Collection to get the items that were removed as an observer

* Use binding instead of reference in GameObjectSingleSetter

* Add provider which checks if a camera is pointing at a collider

* Add PrefabInstantiator to instantiate a game object from a provided prefab

* Add check for empty key in GameObjectMapping to not cause NullReferenceException in inspector

* Add data provider for the main camera

* Add data provider for a component's transform and game object

* Allow data binding for target of ComponentSingleGetter

* Add base class for component data providers and add provider for a transform rotation

* Add component selection when referencing a game object in a Data Binding

* Add setter for transform rotation

* Fix wrong check for if target binding is set in ComponentSingleSetter

* Add TransformPositionSetter to use it instead of obsolete PositionSetter

* Use binding for target of ComponentSingleSetter instead of reference e.g. to feed target from a data provider

* Add editing for Vector fields in context holder editor

* Add GameObjectTransformProvider to get the transform component of a specific game object

* Add FindGameObjectWithTagGetter to find a game object with a specific tag

* Show "Invoke" button in inspector next to context methods for debugging

* Add StrangeIoC extension classes

* Add GestureInput extension commands

* Make string data properties editable in ContextHolderEditor by passing the member type to DrawContextData

* Add setter for a material float property

* Add converter for 3 single numbers to a Vector3

* Only show current value in data provider inspector if active and enabled

* Deactivate prefab of GameObjectItemsSetter on Awake, so it isn't visible even if a scene game object is used

* Setters - Added GameObjectItemsSetter as a non-generic class, using MonoBehaviour as type.

* Lookups - Added lookup to find an item from a collection that has a specific value at the specified path.

* Loaders - Added warning if sprite resource can't be found.

* Core - Using common interface for property, collection and other data providers.

* Data Bind - Using context type drawer in context holder editor. Using enum popup when drawing context data for an enum member.

* Data Bind - Added buttons to context inspector to add/remove items from a collection.

* Collection - Added base AddNewItem and Remove method to data collection.

* Path Drawer - Added maximum path depth to avoid infinite loops.

* Examples - Adjusted ContextProperty example to use GameObjectItemSetter.

* Utils - Not preserving world position when adding child from prefab to game object in UnityUtils.

* Switches - Removed obsolete RangeOption.cs

* Context - ValueChanged event for data node is also thrown when collection changes internally (e.g. item added, removed, cleared). This way child nodes are informed about a parent value change.


Version 1.0.6 (01/05/2016)
**************************

* ADDED: Setters - Added setter which sets the items of a layout group one-by-one instead of all-at-once.

* ADDED: Setters - Added setter too smoothly set the fill amount of an image.

* ADDED: Setters - Added setter for the canvas sorting order.

* ADDED: Providers - Added provider for a material.

* ADDED: Commands - Added commands which trigger on specific input events.

* ADDED: Setters - Added SmoothSlotItemsSetter which fills its slots over time instead of immediately.

* ADDED: Operations - Initializing arguments in LogicalBoolOperation to make sure they are not null.

* ADDED: Setters - Added event when slot of SlotItemsSetter was destroyed. Activating item in slot in case it was hidden before.

* CHANGED: Setters - Making prefab inactive before instantiation in GameObjectItemsSetter. Otherwise new game object is already initialized before its context is set.

* ADDED: Setters - Added base class for animator parameter setters. Added setter for animator speed. Only setting animator trigger if animator is initialized.

* ADDED: Smootheners - Added data providers which smooth a float or long data value.

* ADDED: Objects - Added object which holds a plain boolean value.

* CHANGED: Lookups - More robust value getter in DictionaryLookup.

* ADDED: Lookups - Added lookup for an item and a range of items in a collection.

* ADDED: Formatters - Added string formatter which returns the lowered text of its data value.

* ADDED: Formatters - Added SmoothCollectionChangesFormatter which provides its bound collection one-by-one instead of all-at-once.

* ADDED: Formatters - Added formatter which uses a fallback data value if its primary one isn't set.

* ADDED: Formatters - Added formatter which sets its value depending on a boolean data value.

* CHANGED: Checks - Using utility methods in ComparisonCheck and EqualityCheck, so they don't have to do their own error handling.

* CHANGED: Commands - Made Command class non-abstract so it can be added to a game object.

* ADDED: Utils - Added TryConvertValue method to ReflectionUtils.

* ADDED: Editor - Improved context holder inspector to show the set context in more detail.

* CHANGED: Data Binding - More robust GetValue<T> method. Making sure OnValueChanged is called on initialization.

* ADDED: Context - Added special value changed triggers if data value is of type Collection.

* ADDED: Data Dictionary - Added key and value type properties. Implemented Add(KeyValuePair<TKey, TValue> item) method.

* ADDED: Editor - Added custom DataProviderEditor to show current value.

* FIXED: Getters - Using onValueChanged instead of obsolete onValueChange event of Unity input field in InputFieldGetter.

* FIXED: SlotItemsSetter - Only hiding item game objects that have no slot to show them again when free slots are available again.

* CHANGED: Providers - StringFormatter checks for null reference of its arguments before using them.

* CHANGED: Setters - SingleSetter uses DataBindingOperator base class.

* ADDED: Triggers - Added UnityEventTrigger which triggers a Unity event when a data triggers is invoked.

* CHANGED: Commands - UnityEventCommand will forward argument to the command method instead of calling the method without an argument.

* ADDED: Providers - Added NumberSwitch for selecting an option depending on an integer number.

* FIXED: Providers - Updating value when data dictionary in DictionaryLookup changed.

* ADDED: Core - Added DataTrigger which can be used to inform a context about a one shot event.

* ADDED: Data Bind - Using deepest context if max path depth set for path.

* CHANGED: Presentation - Using IContextOperator interface to inform all scripts that have to know it about a context change.

* ADDED: Data Dictionary - Triggering OnCollectionChanged when value changes.

* FIXED: Core - Getting correct item type for enumerable node in DataNode.

* ADDED: Core - ContextHolder stores path to context to allow relative paths even for collections and initializers with multiple path parts (e.g. GameObjectItem(s)Setter or ContextHolderContextSetter).

* CHANGED: Core - Storing parent node instead only parent object.

* ADDED: Collections - Changed property "Count" to be a data property which updates data bindings if it changes.


Version 1.0.5 (03/12/2015)
**************************

* ADDED: Editor - Showing context data in inspector during runtime.

* ADDED: Editor - Added object field in inspector for data binding if type is Reference.

* ADDED: Operations - Added tween operation to change a number value over time.

* ADDED: Objects - Added simple number object, mainly for testing.

* ADDED: Operations - Added module operation to ArithmeticOperation.

* ADDED: Setters - Added setter for the interactable flag of a CanvasGroup.

* ADDED: Commands - Added base commands for UnityEvents with parameters and multiple target callbacks.

* CHANGED: Setters - Moved ImageMaterialSetter from foundation to UI/Unity.

* ADDED: Setters - Added possibility to hide empty slots and to shift items on remove to SlotItemsSetter.

* ADDED: Setters - Added setter to set the context of a specified context holder.

* ADDED: Setters - Added setter to enable/disable a behaviour.

* ADDED: Operations - Added mapping from string to game object.

* ADDED: Getters - Added provider for transform.position.

* CHANGED: Getters - ComponentSingleGetter overrides OnEnable/OnDisable and calls base method of DataProvider.

* ADDED: Formatters - Added StringToUpperFormatter.

* ADDED: Commands - Catching exception when invoking command to log more helpful message.

* ADDED: Setters - Added setter for the sprite of a sprite renderer.

* ADDED: Setters - Added setter for the material of an image.

* ADDED: Setters - Added setter for a trigger parameter in an animator.

* ADDED: Providers - Added lookup for data dictionary.

* ADDED: Core - Added DataDictionary to have a simple data mapping in contexts.

* ADDED: Context - Added context node which points to an item in an enumerable.

* ADDED: Core - Added constructor for Collection to initialize from IEnumerable.

* CHANGED: Core - Data provider doesn't listen to value changes when not active.

* CHANGED: Setters - SingleSetter catches cast exception to provide more helpful log message.

* ADDED: Core - Added DataBindingType "Reference" to reference a Unity object. Catching cast exception when getting value of data binding. IsInitialized value is set before setting value to be set already on callbacks.

* CHANGED: Formatters - Added bindings for symbols of the DurationFormatter, so it is more generic and can be localized.

* ADDED: Setters - Added items setter for fixed number of provided slots.

* ADDED: Commands - Adding default values for missing parameters when invoking command.

* ADDED: Switches - Added NumberRangeSwitch and base class for range switches.

* ADDED: Providers - Added ColorObject to provide a single color value.

* ADDED: Converters - Added Texture2DToSpriteConverter and base DataConverter.

* ADDED: Context Holder Initializer - Added Reset method to automatically search for context holder on same game object on creation/reset.

* ADDED: Providers - Added BooleanSwitch to provide a different data value depending on a boolean value.

* FIXED: Reflection - Using platform-independent implementations of IsEnum and BaseType.

* ADDED: Core - Added ContextChanged event to ContextHolder.


Version 1.0.4 (25/07/2015)
**************************

* ADDED: Examples - New example with a StringToUpperFormatter which converts a text to upper case.

* FIXED: Core - Private fields of base classes for derived types are not reflected, so base classes have to be searched for data property holders as well.

* CHANGED: StringFormatter - Forwards the format string if string.Format fails.

* ADDED: Setters - Added non-abstract GameObjectItemsSetter to use to instantiate game objects for the items of a collection.
 
* ADDED: Core - Added indexer, IndexOf and RemoveAt method to Collection class.
 
* ADDED: Context Path - Added property to ContextPathAttribute to set a custom display name for the path to be used by the PathPropertyDrawer.
 
* ADDED: Unity UI - Added setter and getter for Toggle.isOn field.

* ADDED: Examples - Added example for doing equality checks and selection for an enum.

* ADDED: Getters - Added getter for the values of a specific enum type.

* ADDED: Type Selection - Added drawer for a selection of a specific type for a base type.

* CHANGED: ItemsSetter - Creating items even if value is just an enumeration and no Collection.

* ADDED: EqualityCheck - Converting string to enum value to check for equality.

* FIXED: PathPropertyDrawer - Custom path wasn't shown initially.

* CHANGED: TextAssetLoader - Adjusted context menu naming to match class name.

* CHANGED: ArithmeticOperation - Renamed parameters from ArgumentA/ArgumentB to First/Second for consistent naming.

* CHANGED: InvertBoolOperation - Renamed Argument to Data for consistent naming.

* CHANGED: TimeFormatter - Renamed to DurationFormatter.

* FIXED: EqualityCheck - Converting data value before doing equality check to consider comparison e.g. to string constant.

* FIXED: PathPropertyDrawer - All arguments of StringFormatter switched to custom path if switching one to it. Storing custom path flag for each property path separately. (Issue: https://bitbucket.org/coeing/data-bind/issue/3/custom-path-is-shown-for-all-path).

* CHANGED: Context Node - Correctly creating context path which starts from context that was determined by the path depth.

* CHANGED: Context Node - Caching master path and contexts at the same time. Depth value defines the path depth, not the context holder depth.

* ADDED: Tests - Added test when using multiple context holders with a master path in between.

* ADDED: Tests - Added unit tests for relative data path.

* ADDED: Diagnostics - Added script to initialize a ContextHolder with a specific context type.

* CHANGED: Data Bind - Clearing cached contexts when hierarchy changed. Otherwise a wrong cached context is used when a game object is placed under a new parent game object, e.g. on lists.


Version 1.0.3 (17/03/2015)
**************************

* CHANGED: Foundation - Moved data providers into Foundation/Providers folder and there into sub folders like Loaders and Operations.

* ADDED: Getters - Using path drop down.

* ADDED: Core - Windows 8 and Windows Phone 8 support. Using Unity platform defines and providing Windows RT implementations for GetField, GetMethod and GetProperty utility methods.

* CHANGED: Examples - Changed command from OnSendMessage to SubmitMessage in InputFieldGetter example.


Version 1.0.2 (28/02/2015)
**************************

* ADDED: Core - Added exceptions if trying to set a read-only property or method.

* CHANGED: Core - DataNode sets value of field or property instead of data property, so even raw members of a context are updated correctly.

* CHANGED: Bindings - Renamed GameObjectSetter to GameObjectSingleSetter.

* CHANGED: Core - Made some methods of context holder virtual to allow deriving from the class if necessary.

* FIXED: Data Bind - Returning path to context if no path is set after #X notation.

* CHANGED: Data Bind - Logging error instead of exception if invalid path was specified (on iOS exceptions caused a crash).

* CHANGED: Editor - Adjusted component menu titles for bindings to be more consistent.

* ADDED: Setters - Added binding to create a child game object from a prefab with a specific context.

* ADDED: GraphicColorSetter - Setter for color of a Graphic component (e.g. Text and Image).

* CHANGED: ArithmeticOperation - Checking second argument for zero before doing division to avoid exception.

* ADDED: Unity UI - Added SelectableInteractableSetter to change if a selectable is interactable depending on a boolean data value.

* ADDED: Operations - Added sprite loader which loads a sprite depending on a string value.

* ADDED: Core - Added log error when context is not derived from Context class, but path is set. This may indicate that the user forgot to derive from Context class.

* ADDED: Context Path - Added property drawer for a context path property.

* CHANGED: Context Holder - Only creating context automatically if explicitly stated and no context is already set in Awake.

Version 1.0.1 (15/02/2015)
**************************

* ADDED: Context - Getting value for context node possible from field as well. 

* FIXED: Property - Null reference check before converting value of data property.

* CHANGED: Collection - Collection base class implements IEnumerable, not IEnumerable<object> so the type of the concrete collection is identified correctly in Linq queries.

* CHANGED: Bindings - Only triggering setters in OnEnable if data binding was already initialized with initial data values.

* FIXED: Command - Safe cast to delegate in Command.

* CHANGED: Foundation - Changed BehaviourSingleGetter/Setter to ComponentSingleGetter/Setter.

* CHANGED: Foundation - Created base class for ItemsSetter to use it for other collection setters as well.

* CHANGED: UnityUI - Made GridItemsSetter for UnityUI more generic to work for all LayoutGroups.

* CHANGED: Active Setter - Changed naming in menu from "Active" to "Active Setter".

* ADDED: Command - Reset method is used to set initial target behaviour.

* ADDED: Bindings - Added several checks, formatters and setters: ComparisonCheck, EqualityCheck, ArithmeticOperation, InvertBoolFormatter, LogicalBoolFormatter, PrependSign, RoundToNearestOperation, TextFileContentFormatter, TimeFormatter, AnimatorBooleanSetter.

* ADDED: NGUI - Added several setters/getters for PopupList, Button and Slider.

* ADDED: Unity UI - Added serveral setters/getters for Slider and Image control.

* ADDED: Foundation - ComponentSingleSetter checks if target is null before updating the component.

* ADDED: Core - Throwing exception if invalid path is passed to context in RegisterListener, RemoveListener or SetValue.

* CHANGED: Game Object Items Setter - Only created items are removed on clear, not all children.