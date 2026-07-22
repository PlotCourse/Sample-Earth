import "./inspect/index.g";
import "../index";
import { DevDashboard } from "./dev-dashboard";

var body = document.getElementsByTagName("body")[0];
body.appendChild(new DevDashboard());
