import {
	IconBrandGithub,
	IconBrandInstagram,
	IconBrandLinkedin,
	IconBrandTwitter,
	IconBrandYoutube,
	IconCoin,
	IconDashboard,
	IconMail,
	IconMapPin,
	IconMapSearch,
	IconUser,
	IconVirusSearch,
  IconWand,
} from "@tabler/icons-react";

export interface PublicNavbarMenu {
	label: string;
	link: string;
}

export const PUBLIC_NAVBAR_MENU: PublicNavbarMenu[] = [
	{ label: "Beranda", link: "/" },
	{ label: "Tentang", link: "/about" },
];

export interface PublicFooterLink {
	title: string;
	links: PublicFooterLinkItem[];
}

export interface PublicFooterLinkItem {
	label: string;
	link: string;
}

export const PUBLIC_FOOTER_LINKS: PublicFooterLink[] = [
	{
		title: "Kodesiana",
		links: [
			{
				label: "Kodesiana",
				link: "https://www.kodesiana.com",
			},
			{
				label: "Open Source",
				link: "https://www.kodesiana.com/open-source/",
			},
			{
				label: "Bug Hunt",
				link: "https://www.kodesiana.com/bug-hunt-program/",
			},
			{
				label: "Kebijakan Privasi",
				link: "https://www.kodesiana.com/privacy-policy",
			},
		],
	},
	{
		title: "AI",
		links: [
			{ label: "Open Dataset", link: "https://ai.kodesiana.com/dataset" },
			{ label: "AI on Edge", link: "https://ai.kodesiana.com/edge-ai" },
			{ label: "AI Workbench", link: "https://ai.kodesiana.com/app" },
		],
	},
	{
		title: "Edu",
		links: [
			{
				label: "KFlearning",
				link: "https://www.kodesiana.com/projects/kflearning",
			},
			{
				label: "Riset dan Penelitian",
				link: "https://www.kodesiana.com/research",
			},
			{
				label: "Pelatihan",
				link: "https://www.kodesiana.com/training",
			},
		],
	},
];

export interface FooterSocialLink {
	ariaLabel: string;
	link: string;
	icon: any;
}

export const FOOTER_SOCIAL_LINKS: FooterSocialLink[] = [
	{
		ariaLabel: "Email",
		link: "mailto:fahmi@kodesiana.com",
		icon: IconMail,
	},
	{
		ariaLabel: "Donasi",
		link: "https://saweria.co/fahminlb33",
		icon: IconCoin,
	},
	{
		ariaLabel: "Youtube",
		link: "https://www.youtube.com/c/FahmiNoorFiqri",
		icon: IconBrandYoutube,
	},
	{
		ariaLabel: "GitHub",
		link: "https://github.com/fahminlb33",
		icon: IconBrandGithub,
	},
	{
		ariaLabel: "LinkedIn",
		link: "https://www.linkedin.com/in/fahmi-noor-fiqri",
		icon: IconBrandLinkedin,
	},
	{
		ariaLabel: "Instagram",
		link: "https://www.instagram.com/fahminoorfiqri",
		icon: IconBrandInstagram,
	},
	{
		ariaLabel: "Twitter",
		link: "https://twitter.com/fahminoorfiqri",
		icon: IconBrandTwitter,
	},
];

export interface AppSidebarMenu {
	link: string;
	label: string;
	icon: any;
}

export const APP_SIDEBAR_MENU: AppSidebarMenu[] = [
	{ link: "/app/dashboard", label: "Dashboard", icon: IconDashboard },
	{
		link: "/app/disease-predictions",
		label: "Analisis Penyakit",
		icon: IconVirusSearch,
	},
	{
		link: "/app/land-observations",
		label: "Analisis Lahan",
		icon: IconMapSearch,
	},
  { link: "/app/recommendation", label: "Rekomendasi", icon: IconWand },
	{ link: "/app/maps", label: "Pemetaan", icon: IconMapPin },
	{ link: "/app/users", label: "Pengguna", icon: IconUser },
];
