import { ChangePasswordModal } from "./ChangePasswordModal";
import { HelpLandObservationModal } from "./HelpLandObservationModal";

const modals = {
	"change-password": ChangePasswordModal,
	"help-land-observation": HelpLandObservationModal,
};

export enum AvailableModal {
	ChangePassword = "change-password",
	HelpLandObservation = "help-land-observation",
}

export default modals;
