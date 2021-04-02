﻿using System;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.Extensions;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar
{
	abstract class BaseSnackBarView : UIView
	{
		public BaseSnackBarView(NativeSnackBar snackBar) => SnackBar = snackBar;

		public virtual UIView? ParentView => SnackBar.ParentController != null
			? SnackBar.ParentController.View
			: UIApplication.SharedApplication.KeyWindow;

		protected NativeSnackBar SnackBar { get; }

		protected UIStackView? StackView { get; set; }

		public virtual void Dismiss() => RemoveFromSuperview();

		public virtual void Setup()
		{
			Initialize();
			ConstrainInParent();
			ConstrainChildren();
		}

		protected virtual void ConstrainChildren()
		{
		}

		protected virtual void ConstrainInParent()
		{
			_ = StackView ?? throw new InvalidOperationException("BaseSnackBarView.Initialize() not called");
			_ = ParentView ?? throw new System.NullReferenceException();

			this.SafeBottomAnchor().ConstraintEqualTo(GetBottomAnchor(), -SnackBar.Layout.MarginBottom).Active = true;
			this.SafeTopAnchor().ConstraintGreaterThanOrEqualTo(GetTopAnchor(), SnackBar.Layout.MarginTop).Active = true;
			this.SafeLeadingAnchor().ConstraintGreaterThanOrEqualTo(ParentView.SafeLeadingAnchor(), SnackBar.Layout.MarginLeft).Active = true;
			this.SafeTrailingAnchor().ConstraintLessThanOrEqualTo(ParentView.SafeTrailingAnchor(), -SnackBar.Layout.MarginRight).Active = true;
			this.SafeCenterXAnchor().ConstraintEqualTo(ParentView.SafeCenterXAnchor()).Active = true;

			StackView.SafeLeadingAnchor().ConstraintEqualTo(this.SafeLeadingAnchor(), SnackBar.Layout.PaddingLeft).Active = true;
			StackView.SafeTrailingAnchor().ConstraintEqualTo(this.SafeTrailingAnchor(), -SnackBar.Layout.PaddingRight).Active = true;
			StackView.SafeBottomAnchor().ConstraintEqualTo(this.SafeBottomAnchor(), -SnackBar.Layout.PaddingBottom).Active = true;
			StackView.SafeTopAnchor().ConstraintEqualTo(this.SafeTopAnchor(), SnackBar.Layout.PaddingTop).Active = true;
		}

		protected virtual NSLayoutYAxisAnchor GetBottomAnchor()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || SnackBar.ParentController == null)
			{
				_ = ParentView ?? throw new System.NullReferenceException();
				return ParentView.SafeBottomAnchor();
			}

			return SnackBar.ParentController.BottomLayoutGuide.GetTopAnchor();
		}

		protected virtual NSLayoutYAxisAnchor GetCenterYAnchor()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || SnackBar.ParentController == null)
			{
				_ = ParentView ?? throw new NullReferenceException();
				return ParentView.SafeCenterYAnchor();
			}

			return SnackBar.ParentController.View?.CenterYAnchor ?? throw new NullReferenceException();
		}

		protected virtual NSLayoutYAxisAnchor GetTopAnchor()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || SnackBar.ParentController == null)
			{
				_ = ParentView ?? throw new NullReferenceException();
				return ParentView.SafeTopAnchor();
			}

			return SnackBar.ParentController.TopLayoutGuide.GetBottomAnchor();
		}

		protected virtual void Initialize()
		{
			StackView = new UIStackView();
			AddSubview(StackView);
			StackView.Axis = UILayoutConstraintAxis.Horizontal;
			StackView.TranslatesAutoresizingMaskIntoConstraints = false;
			StackView.Spacing = SnackBar.Layout.Spacing;
			if (SnackBar.Appearance.Background != NativeSnackBarAppearance.DefaultColor)
			{
				StackView.BackgroundColor = SnackBar.Appearance.Background;
			}

			TranslatesAutoresizingMaskIntoConstraints = false;
		}
	}
}