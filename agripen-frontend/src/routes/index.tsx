import { createBrowserRouter, redirect, Navigate } from "react-router-dom";

import AppLayout from "@/layouts/AppLayout";
import AuthLayout from "@/layouts/AuthLayout";
import PublicLayout from "@/layouts/PublicLayout";

import * as RootPages from "@/pages";

import * as AuthPages from "@/pages/auth";

import * as AppDashboardPages from "@/pages/app/dashboard";
import * as AppDiseasesPredictionsPages from "@/pages/app/disease-predictions";
import * as AppLandObservationsPages from "@/pages/app/land-observations";
import * as RecommendationPages from "@/pages/app/recommendation";
import * as AppMapsPages from "@/pages/app/maps";
import * as AppUsersPages from "@/pages/app/users";

import { useAuth } from "@/hooks/useAuth";

const AuthRoute = () => {
	const { user } = useAuth();
	if (user) {
		return <Navigate to="/app/dashboard" />;
	}

	return <AuthLayout />;
};

const ProtectedAppRoute = () => {
	const { user } = useAuth();
	if (!user) {
		return <Navigate to="/auth/login" />;
	}

	return <AppLayout />;
};

const router = createBrowserRouter([
	{
		element: <PublicLayout />,
		errorElement: <RootPages.ErrorPage />,
		children: [
			{
				index: true,
				element: <RootPages.Home />,
			},
			{
				path: "/about",
				element: <RootPages.About />,
			},
		],
	},
	{
		path: "/auth",
		element: <AuthRoute />,
		errorElement: <RootPages.ErrorPage />,
		children: [
			{
				index: true,
				loader: () => redirect("/auth/login"),
			},
			{
				path: "login",
				element: <AuthPages.Login />,
			},
		],
	},
	{
		path: "/app",
		element: <ProtectedAppRoute />,
		// errorElement: <RootPages.ErrorPage />,
		children: [
			{
				index: true,
				loader: () => redirect("/app/dashboard"),
			},
			{
				path: "dashboard",
				loader: AppDashboardPages.DashboardLoader,
				element: <AppDashboardPages.Dashboard />,
			},
			{
				path: "disease-predictions",
				children: [
					{
						index: true,
						loader: AppDiseasesPredictionsPages.ListLoader,
						element: <AppDiseasesPredictionsPages.List />,
					},
					{
						path: "upload",
						element: <AppDiseasesPredictionsPages.Upload />,
					},
					{
						path: ":id",
						loader: AppDiseasesPredictionsPages.AnalysisLoader,
						element: <AppDiseasesPredictionsPages.Analysis />,
					},
				],
			},
			{
				path: "land-observations",
				children: [
					{
						index: true,
						loader: AppLandObservationsPages.ListLoader,
						element: <AppLandObservationsPages.List />,
					},
					{
						path: ":id",
						loader: AppLandObservationsPages.AnalysisLoader,
						element: <AppLandObservationsPages.Analysis />,
					},
				],
			},
      {
				path: "recommendation",
        children: [
					{
						index: true,
						loader: RecommendationPages.ListLoader,
				    element: <RecommendationPages.List />,
					},
					{
						path: ":id",
						loader: RecommendationPages.DetailLoader,
						element: <RecommendationPages.Detail />,
					},
				],
        
			},
			{
				path: "maps",
				element: <AppMapsPages.Home />,
				children: [
					{
						index: true,
						loader: () => redirect("/app/maps/weather"),
					},
					{
						path: "weather",
						loader: AppMapsPages.WeatherLoader,
						element: <AppMapsPages.Weather />,
					},
					{
						path: "disease-predictions",
						loader: AppMapsPages.DiseasePredictionsLoader,
						element: <AppMapsPages.DiseasePredictions />,
					},
					{
						path: "land-observations",
						loader: AppMapsPages.LandObservationsLoader,
						element: <AppMapsPages.LandObservations />,
					},
				],
			},
			{
				path: "users",
				children: [
					{
						index: true,
						loader: AppUsersPages.ListLoader,
						element: <AppUsersPages.List />,
					},
					{
						path: "create",
						element: <AppUsersPages.Create />,
					},
				],
			},
		],
	},
	// {
	// 	path: "*",
	// 	element: <RootPages.ErrorPage />,
	// },
]);

export default router;
